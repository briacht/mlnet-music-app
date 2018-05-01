using IntelligentDemo.Convertors;
using IntelligentDemo.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IntelligentDemo.Pages
{
    public partial class DotnetFeedback : UserControl, IDisposable
    {
        private LightController _lightController;
        private FeedbackService.Feedback[] _data;
        private int _currentIndex;

        public DotnetFeedback(SongController controller)
        {
            InitializeComponent();

            controller.BarStarted += (_, e) =>
            {
                if (e.BarNumber > 1 && e.BarNumber % 2 == 1)
                {
                    _currentIndex = (_currentIndex + 1) % _data.Length;
                    Refresh();
                }
            };

            _lightController = new LightController();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var svc = new FeedbackService();
            _data = svc.GetFeedback().ToArray();
            _currentIndex = 0;
            Refresh();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < _data.Length - 1)
            {
                _currentIndex++;
                Refresh();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                Refresh();
            }
        }

        private void Refresh()
        {
            var item = _data[_currentIndex];
            DataContext = item;
            _lightController.SetColor(ScoreToColorConvertor.Convert(item.Score));
        }

        public void Dispose()
        {
            _lightController.Dispose();
        }
    }
}
