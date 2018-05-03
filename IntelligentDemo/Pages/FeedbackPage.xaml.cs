using IntelligentDemo.Convertors;
using IntelligentDemo.Models;
using IntelligentDemo.Models.Music;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IntelligentDemo.Pages
{
    public partial class FeedbackPage : UserControl, IDisposable
    {
        private LightController _lightController = new LightController();
        private FeedbackService _feedbackService = new FeedbackService();
        private SongController _songController;

        public FeedbackPage(SongController controller)
        {
            _songController = controller;

            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await _feedbackService.GetFeedback();
            DetailsList.ItemsSource = data;
            DetailsList.SelectedIndex = 0;

            _songController.BarStarted += SongController_BarStarted;
        }

        private void SongController_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (e.BarNumber > 1 && e.BarNumber % 4 == 1)
            {
                DetailsList.SelectedIndex = (DetailsList.SelectedIndex + 1) % DetailsList.Items.Count;
            }
        }

        public void Dispose()
        {
            _lightController.Dispose();
        }

        private void DetailsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.Cast<Feedback>().FirstOrDefault();
            if(item != null)
            {
                _lightController.SetColor(ScoreToColorConvertor.Convert(item.Score));
                DetailsList.ScrollIntoView(item);
            }
        }
    }
}
