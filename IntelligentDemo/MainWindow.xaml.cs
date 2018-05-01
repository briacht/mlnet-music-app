using IntelligentDemo.Models;
using IntelligentDemo.Pages;
using System;
using System.Windows;

namespace IntelligentDemo
{
    public partial class MainWindow : Window, IDisposable
    {
        private SongController _controller = new SongController();

        public MainWindow()
        {
            InitializeComponent();

            Content.Content = new CameraPage(_controller);
            //Content.Content = new DotnetFeedback(_controller);

            //var x = new CameraPage();
            //x.Show();
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
