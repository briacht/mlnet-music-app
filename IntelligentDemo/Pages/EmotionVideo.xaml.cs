using IntelligentDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace IntelligentDemo.Pages
{
    public sealed partial class EmotionVideo : Page
    {
        private static readonly Dictionary<string, IEnumerable<NoteCommand>> _bassLines = InitializeBassLines();

        private MediaCapture _webcam;
        private int _barsPerImage = 4;

        public ObservableCollection<AnalyzedImage> Queue { get; } = new ObservableCollection<AnalyzedImage>();

        public EmotionVideo()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeCameraAsync();
        }

        private void Controller_BarStarted(object sender, BarStartedEventArgs e)
        {
            // First bar of this set of bars and there is something queued
            if (CarouselControl.Items.Count > 0 && e.BarNumber % _barsPerImage == 1)
            {
                CarouselControl.SelectedIndex = CarouselControl.Items.Count - 1;
            }

            // Last bar of this set of bars and there is something queued
            if (CarouselControl.Items.Count - 1 > CarouselControl.SelectedIndex && e.BarNumber % _barsPerImage == 0)
            {
                _songController.SetNextBassBar(_bassLines[Queue.Last().Emotion.ToLower()]);
            }

            // Two bars left in this set of bars
            if (e.BarNumber % _barsPerImage == _barsPerImage - 1)
            {
                CaptureNextImage();
            }
        }

        private SongController _songController;
        public SongController Controller
        {
            get { return _songController; }
            set
            {
                _songController = value;
                _songController.BarStarted += Controller_BarStarted;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var emotion = (string)button.Content;
            var notes = _bassLines[emotion];
            Controller.SetNextBassBar(notes);
        }

        private async Task InitializeCameraAsync()
        {
            if (_webcam == null)
            {
                // Get the camera devices
                var cameraDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                // try to get the front facing device
                var frontFacingDevice = cameraDevices[1]; 
                //.FirstOrDefault(c => c.EnclosureLocation?.Panel == Windows.Devices.Enumeration.Panel.Front);

                // Create MediaCapture
                _webcam = new MediaCapture();

                // Initialize MediaCapture and settings
                await _webcam.InitializeAsync(
                    new MediaCaptureInitializationSettings
                    {
                        VideoDeviceId = frontFacingDevice.Id
                    });

                // Set the preview source for the CaptureElement
                LiveVideo.Source = _webcam;

                // Start viewing through the CaptureElement 
                await _webcam.StartPreviewAsync();
            }
        }

        private async void CaptureNextImage()
        {
            const string subscriptionKey = "";
            const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";

            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=emotion";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            // Create the file that we're going to save the photo to.
            var file = await KnownFolders.SavedPictures.CreateFileAsync($"CamCapture_{Queue.Count + 1}.jpg", CreationCollisionOption.ReplaceExisting);

            CaptureLight.Visibility = Visibility.Visible;
            await Task.Delay(250);
            CaptureLight.Visibility = Visibility.Collapsed;
            await Task.Delay(250);

            CaptureLight.Visibility = Visibility.Visible;
            await Task.Delay(250);
            CaptureLight.Visibility = Visibility.Collapsed;
            await Task.Delay(250);

            CaptureLight.Fill = new SolidColorBrush(Windows.UI.Colors.White);
            CaptureLight.Visibility = Visibility.Visible;

            await _webcam.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);
            var bitmap = await LoadImageAsync(file);
            StillCapture.Source = bitmap;
            Status.Text = "Analyzing image...";
            ImageProcessing.Visibility = Visibility.Visible;

            byte[] byteData = await Task.Run(() => GetImageAsByteArray(file.Path));

            var pause = Task.Delay(500);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FaceDetectionResult[]>(resultString);

                if (result.Any())
                {
                    var emoti = result.First().FaceAttributes.Emotion.OrderByDescending(t => t.Value).First().Key;
                    await pause;
                    Queue.Add(new AnalyzedImage { Emotion = emoti, BitmapImage = bitmap });
                    Status.Text = $"Detected {emoti.ToLower()}";
                }
                else
                {
                    await pause;
                    Status.Text = $"Did not detect a face";
                }

            }

            await Task.Delay(2000);
            CaptureLight.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            CaptureLight.Visibility = Visibility.Collapsed;
            ImageProcessing.Visibility = Visibility.Collapsed;
        }

        private static async Task<BitmapImage> LoadImageAsync(StorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);

            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        private static Dictionary<string, IEnumerable<NoteCommand>> InitializeBassLines()
        {
            var result = new Dictionary<string, IEnumerable<NoteCommand>>();

            result["anger"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 16, Velocity = 127, Position = 1},
            };

            result["contempt"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 13},
            };

            result["disgust"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            result["fear"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 3},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 7},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 9},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 11},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 13},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 15},
            };

            result["happiness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 50, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 48, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 50, Duration = 4, Velocity = 127, Position = 13},
            };

            result["neutral"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 8, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 48, Duration = 8, Velocity = 127, Position = 9},
            };

            result["sadness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            result["surprise"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 60, Duration = 8, Velocity = 127, Position = 9},
            };

            return result;
        }
    }

    public class AnalyzedImage
    {
        public BitmapImage BitmapImage { get; set; }
        public string Emotion { get; set; }
    }

    public class FaceDetectionResult
    {
        public String FaceId { get; set; }
        public Attributes FaceAttributes { get; set; }

        public class Attributes
        {
            public Emotions Emotion { get; set; }
        }

        public class Emotions : Dictionary<string, decimal>
        {
            public decimal Anger { get { return this["Anger"]; } set { this["Anger"] = value; } }
            public decimal Contempt { get { return this["Contempt"]; } set { this["Contempt"] = value; } }
            public decimal Disgust { get { return this["Disgust"]; } set { this["Disgust"] = value; } }
            public decimal Fear { get { return this["Fear"]; } set { this["Fear"] = value; } }
            public decimal Happiness { get { return this["Happiness"]; } set { this["Happiness"] = value; } }
            public decimal Neutral { get { return this["Neutral"]; } set { this["Neutral"] = value; } }
            public decimal Sadness { get { return this["Sadness"]; } set { this["Sadness"] = value; } }
            public decimal Surprise { get { return this["Surprise"]; } set { this["Surprise"] = value; } }
        }
    }
}
