using IntelligentDemo.Models;
using IntelligentDemo.Models.Music;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IntelligentDemo.Pages
{
    public partial class MusicPage : UserControl
    {
        private const double DEFAULT_VOLUME = 0.5;

        private SongController _songController;
        bool playing;
        private List<NoteCommand> _melody = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 74, Duration = 2, Velocity = 127, Position = 5},
                        new NoteCommand{ Note = 76, Duration = 2, Velocity = 127, Position = 7},
                        new NoteCommand{ Note = 76, Duration = 4, Velocity = 127, Position = 9},
                        new NoteCommand{ Note = 74, Duration = 2, Velocity = 127, Position = 13},
                        new NoteCommand{ Note = 72, Duration = 2, Velocity = 127, Position = 15},
                    };

        public MusicPage(SongController controller)
        {
            InitializeComponent();
            _songController = controller;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VolumeSlider.Value = DEFAULT_VOLUME * 100;

            _songController.BarStarted += _songController_BarStarted;
        }

        private void _songController_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (playing)
            {
                if (e.BarNumber % 4 == 0)
                {
                    

                    _songController.SetNextMelodyBar(_melody);
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _songController?.SetMelodyVolume(e.NewValue / 100);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (!playing)
            {
                //if (DetailsList.Items.Count > 0)
                //{
                //    if (DetailsList.SelectedIndex < 0)
                //    {
                //        DetailsList.SelectedIndex = 0;
                //    }

                //    SetNext(DetailsList.SelectedIndex);
                //}
                _songController.SetNextMelodyBar(_melody);

                playing = true;
                PlayButton.Background = new SolidColorBrush(Color.FromRgb(0x10, 0x7c, 0x10));

                _songController.Start();
            }
            else
            {
                playing = false;
                _songController.SetNextMelodyBar(Array.Empty<NoteCommand>());
                PlayButton.Background = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));
            }
        }
    }
}
