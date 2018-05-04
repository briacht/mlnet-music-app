using IntelligentDemo.Models;
using IntelligentDemo.Models.Music;
using IntelligentDemo.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IntelligentDemo
{
    public partial class MainWindow : Window, IDisposable
    {
        private SongController _controller = new SongController();
        private Lazy<MusicPage> _musicPage;
        private Lazy<CameraPage> _cameraPage;
        private Lazy<FeedbackPage> _feedbackPage;
        private Lazy<TwitterPage> _twitterPage;

        public MainWindow()
        {
            InitializeComponent();

            _cameraPage = new Lazy<CameraPage>(() => new CameraPage(_controller));
            _feedbackPage = new Lazy<FeedbackPage>(() => new FeedbackPage(_controller));
            _twitterPage = new Lazy<TwitterPage>(() => new TwitterPage(_controller));
            _musicPage = new Lazy<MusicPage>(() => new MusicPage(_controller));

            Content.Content = new SplitPage(new UserControl[] { _cameraPage.Value, _twitterPage.Value, _musicPage.Value, _feedbackPage.Value });
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _controller.Start();
            PlayButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _controller.Pause();
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        public void Dispose()
        {
            _controller.Dispose();

            if (_feedbackPage.IsValueCreated) _feedbackPage.Value.Dispose();
            if (_twitterPage.IsValueCreated) _twitterPage.Value.Dispose();
        }
    }
}
