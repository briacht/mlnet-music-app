using IntelligentDemo.Models;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntelligentDemo.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmotionTwitter : Page
    {
        private Dictionary<string, IEnumerable<NoteCommand>> _bars = new Dictionary<string, IEnumerable<NoteCommand>>();

        public EmotionTwitter()
        {
            this.InitializeComponent();

            // TODO Makes these percussion bars
            _bars["Anger"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 16, Velocity = 127, Position = 1},
            };

            _bars["Contempt"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 13},
            };

            _bars["Disgust"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            _bars["Fear"] = new List<NoteCommand>
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

            _bars["Happiness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 50, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 48, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 50, Duration = 4, Velocity = 127, Position = 13},
            };

            _bars["Neutral"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 8, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 48, Duration = 8, Velocity = 127, Position = 9},
            };

            _bars["Sadness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            _bars["Surprise"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 60, Duration = 8, Velocity = 127, Position = 9},
            };
        }

        public SongController Controller { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var emotion = (string)button.Content;
            var notes = _bars[emotion];

            // TODO Send percussion bar to SongController
        }
    }
}
