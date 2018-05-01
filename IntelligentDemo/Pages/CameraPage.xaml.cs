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
        private SongController _controller;
        private EmotionService _emotionService = new EmotionService();
        private int? _currentIndex;
        private int? _nextIndex;

        public CameraPage(SongController controller)
        {
            InitializeComponent();

            _controller = controller;
            _controller.BarStarted += _controller_BarStarted;
            DataContext = this;
        }

        public ObservableCollection<FeedbackViewModel> Images { get; set; } = new ObservableCollection<FeedbackViewModel>();

        private void _controller_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (e.BarNumber % 4 == 1)
            {
                if (_currentIndex != null)
                {
                    Images[_currentIndex.Value].Playing = false;
                }

                if (_nextIndex != null)
                {
                    Images[_nextIndex.Value].Playing = true;
                    _currentIndex = _nextIndex;
                    _nextIndex = null;
                }

                
            }

            if(e.BarNumber % 4 == 0)
            {
                var next = _currentIndex.HasValue ?
                    Images.Skip(_currentIndex.Value + 1).Where(i => _bassLines.ContainsKey(i.Emotion)).FirstOrDefault()
                        ?? Images.Take(_currentIndex.Value).Where(i => _bassLines.ContainsKey(i.Emotion)).FirstOrDefault()
                        ?? Images.Where(i => _bassLines.ContainsKey(i.Emotion)).FirstOrDefault()
                    : Images.Where(i => _bassLines.ContainsKey(i.Emotion)).FirstOrDefault();

                if (next != null)
                {
                    _nextIndex = Images.IndexOf(next);
                    _controller.SetNextBassBar(_bassLines[next.Emotion]);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var cam = EncoderDevices.FindDevices(EncoderDeviceType.Video).Last();
            WebcamViewer.VideoDevice = cam;
            WebcamViewer.ImageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VideoCaptures");
            WebcamViewer.StartPreview();
        }

        private async void Capture_Click(object sender, RoutedEventArgs e)
        {
            var path = WebcamViewer.TakeSnapshot();
            var img = CropImage(path);
            var bmp = new BitmapImage(new Uri(img));

            var result = new FeedbackViewModel { Emotion = "Analyzing...", Image = bmp };
            result.Emotion = await _emotionService.DetectEmotion(path);
            Images.Add(result);
        }

        private static string CropImage(string filePath)
        {
            var snap = new BitmapImage(new Uri(filePath));
            var dimension = Convert.ToInt32(snap.Width < snap.Height ? snap.Width : snap.Height);

            Int32Rect rect;
            if(snap.Width < snap.Height * 1.33)
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
                        : new SolidColorBrush(Color.FromRgb(0, 0, 0));
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
