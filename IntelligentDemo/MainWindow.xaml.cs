using IntelligentDemo.Models;
using IntelligentDemo.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IntelligentDemo
{
    public partial class MainWindow : Window, IDisposable
    {
        private SongController _controller = new SongController();
        private Lazy<CameraPage> _cameraPage;
        private Lazy<FeedbackPage> _feedbackPage;
        private Lazy<TwitterPage> _twitterPage;

        public MainWindow()
        {
            InitializeComponent();

            _cameraPage = new Lazy<CameraPage>(() => new CameraPage(_controller));
            _feedbackPage = new Lazy<FeedbackPage>(() => new FeedbackPage(_controller));
            _twitterPage = new Lazy<TwitterPage>(() => new TwitterPage(_controller));

            Content.Content = new SplitPage(new UserControl[] {_cameraPage.Value, _feedbackPage.Value, _twitterPage.Value });
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _controller.Start();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _controller.Pause();
        }

        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}
