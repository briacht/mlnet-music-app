using IntelligentDemo.Models;
using Microsoft.Expression.Encoder.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IntelligentDemo.Pages
{
    public partial class CameraPage : UserControl
    {
        private static readonly Dictionary<string, IEnumerable<NoteCommand>> _bassLines = InitializeBassLines();
        private EmotionService _emotionService = new EmotionService();
        private SongController _songController;
        private int? _nextIndex;
        bool processingAutoMove;

        public CameraPage(SongController controller)
        {
            InitializeComponent();

            _songController = controller;
        }

        public ObservableCollection<FeedbackViewModel> Images { get; set; } = new ObservableCollection<FeedbackViewModel>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var cam = EncoderDevices.FindDevices(EncoderDeviceType.Video).Last();
            WebcamViewer.VideoDevice = cam;
            WebcamViewer.ImageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VideoCaptures");
            WebcamViewer.StartPreview();

            DetailsList.ItemsSource = Images;

            _songController.BarStarted += Controller_BarStarted;
        }

        private void Controller_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (e.BarNumber % 4 == 1)
            {
                if (_nextIndex != null)
                {
                    processingAutoMove = true;
                    DetailsList.SelectedIndex = _nextIndex.Value;
                    _nextIndex = null;
                    processingAutoMove = false;
                }
            }

            if (e.BarNumber % 4 == 0 && Images.Any())
            {
                SetNext((DetailsList.SelectedIndex + 1) % DetailsList.Items.Count);
            }
        }

        private void SetNext(int index)
        {
            _nextIndex = index;

            var next = Images[_nextIndex.Value];
            if (_bassLines.ContainsKey(next.Emotion))
            {
                _songController.SetNextBassBar(_bassLines[next.Emotion]);
            }
            else
            {
                _songController.SetNextBassBar(_bassLines.Values.ElementAt(_nextIndex.HasValue ? _nextIndex.Value % _bassLines.Count : 0));
            }
        }

        private void DetailsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                DetailsList.ScrollIntoView(e.AddedItems[0]);

                if (!processingAutoMove)
                {
                    SetNext(DetailsList.Items.IndexOf(e.AddedItems[0]));
                }
            }
        }

        private async void Capture_Click(object sender, RoutedEventArgs e)
        {
            var path = WebcamViewer.TakeSnapshot();
            var img = CropImage(path);
            var bmp = new BitmapImage(new Uri(img));

            var result = new FeedbackViewModel { Emotion = "Analyzing...", Image = bmp };
            result.Emotion = await _emotionService.DetectEmotionFromFile(path);
            Images.Add(result);

            // If it was the first image, select it
            if(Images.Count == 1)
            {
                DetailsList.SelectedIndex = 0;
            }
        }

        private static string CropImage(string filePath)
        {
            var snap = new BitmapImage(new Uri(filePath));
            var dimension = Convert.ToInt32(snap.Width < snap.Height ? snap.Width : snap.Height);

            Int32Rect rect;
            if (snap.Width < snap.Height * 1.33)
            {
                // Padding is on top/bottom
                var width = Convert.ToInt32(snap.Width);
                var height = Convert.ToInt32(snap.Width * 0.75);

                var x = 0;
                var y = (Convert.ToInt32(snap.Height) - height) / 2;

                rect = new Int32Rect(x, y, width, height);
            }
            else
            {
                // Padding is on sides
                var width = Convert.ToInt32(snap.Height * 1.33);
                var height = Convert.ToInt32(snap.Height);

                var x = (Convert.ToInt32(snap.Width) - width) / 2;
                var y = 0;

                rect = new Int32Rect(x, y, width, height);
            }

            var crop = new CroppedBitmap(snap, rect);

            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(crop));

            var path = filePath.Replace(".Jpeg", "_cropped.Jpeg");

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            return path;
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
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 7},
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 13},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 15},
            };

            result["neutral"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            result["sadness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            result["surprise"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 13},
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 15},
            };

            return result;
        }

        public class FeedbackViewModel : INotifyPropertyChanged
        {
            private ImageSource _image;
            private string _emotion;
            private bool _playing;

            public ImageSource Image
            {
                get { return _image; }
                set
                {
                    _image = value;
                    NotifyPropertyChanged();
                }
            }

            public string Emotion
            {
                get { return _emotion; }
                set
                {
                    _emotion = value;
                    NotifyPropertyChanged();
                }
            }

            public bool Playing
            {
                get { return _playing; }
                set
                {
                    _playing = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(Background));
                }
            }

            public Brush Background
            {
                get
                {
                    return _playing
                        ? new SolidColorBrush(Color.FromRgb(0, 255, 0))
                        : new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
