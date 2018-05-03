using IntelligentDemo.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IntelligentDemo.Pages
{
    /// <summary>
    /// Interaction logic for MusicPage.xaml
    /// </summary>
    public partial class MusicPage : UserControl
    {
        private SongController _songController;

        public MusicPage(SongController controller)
        {
            InitializeComponent();
            _songController = controller;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _songController.BarStarted += _songController_BarStarted;
        }

        private void _songController_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (e.BarNumber % 4 == 0)
            {
                var melody = new List<NoteCommand>
                {
                    new NoteCommand{ Note = 72, Duration = 4, Velocity = 70, Position = 1},
                    new NoteCommand{ Note = 74, Duration = 2, Velocity = 70, Position = 5},
                    new NoteCommand{ Note = 76, Duration = 2, Velocity = 70, Position = 7},
                    new NoteCommand{ Note = 76, Duration = 4, Velocity = 70, Position = 9},
                    new NoteCommand{ Note = 74, Duration = 2, Velocity = 70, Position = 13},
                    new NoteCommand{ Note = 72, Duration = 2, Velocity = 70, Position = 15},
                };

                _songController.SetNextMelodyBar(melody);
            }
        }
    }
}
