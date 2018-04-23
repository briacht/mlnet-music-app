using IntelligentDemo.Models;
using IntelligentDemo.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IntelligentDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IDisposable
    {
        private SongController _controller = new SongController();

        public MainPage()
        {
            this.InitializeComponent();

            this.EmotionTwitter.Controller = _controller;
            this.EmotionVideo.Controller = _controller;
            this.TextAnalysis.Controller = _controller;
        }

        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}
