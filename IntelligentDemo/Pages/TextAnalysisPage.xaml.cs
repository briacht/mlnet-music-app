using IntelligentDemo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntelligentDemo.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TextAnalysisPage : Page
    {

        public TextAnalysisPage()
        {
            this.InitializeComponent();
            this.ViewModel = new FeedbackViewModel();
        }

        public FeedbackViewModel ViewModel { get; set; }
        public SongController Controller { get; set; }
    }


    public class FeedbackViewModel : INotifyPropertyChanged
    {
        private FeedbackService.Feedback[] _all;
        private int _currentIndex = 0;
        private int _nextIndex = 1;
        private DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
        //private Controller _controller = new Controller();

        public FeedbackViewModel()
        {
            _all = new FeedbackService().GetFeedback().ToArray();
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            _currentIndex = _nextIndex;
            _nextIndex++;
            _nextIndex = _nextIndex % _all.Length;

            NotifyPropertyChanged("CurrentIndex");
            NotifyPropertyChanged("Current");
            NotifyPropertyChanged("CurrentColor");
            NotifyPropertyChanged("NextIndex");
            NotifyPropertyChanged("Next");
            //_controller.setNextBassNote(GetNote(_all[_currentIndex]));
        }

        private byte GetNote(FeedbackService.Feedback feedback)
        {
            if (Current.Score > 0.75)
            {
                return 40;
            }
            else if (Current.Score < 0.25)
            {
                return 36;
            }
            else
            {
                return 38;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CurrentIndex => _currentIndex;
        public int NextIndex => _nextIndex;

        public FeedbackService.Feedback Current => _all[_currentIndex];
        public FeedbackService.Feedback Next => _all[_nextIndex];

        public Brush CurrentColor
        {
            get
            {
                if (Current.Score > 0.75)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                }
                else if (Current.Score < 0.25)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 216, 0));
                }
            }
        }

        public IEnumerable<FeedbackService.Feedback> All => _all;
    }

    public class ScoreToColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var score = value as double?;
            if (score == null || targetType != typeof(Brush))
            {
                return null;
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, System.Convert.ToByte(255 * (1 - score.Value)), System.Convert.ToByte(255 * score.Value), 0));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ScoreToWidthConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var score = value as double?;
            if (score == null || targetType != typeof(double))
            {
                return null;
            }
            else
            {
                return score.Value * 350;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
