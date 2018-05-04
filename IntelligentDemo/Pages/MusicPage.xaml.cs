using IntelligentDemo.Models;
using IntelligentDemo.Models.Services;
using IntelligentDemo.Services;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IntelligentDemo.Pages
{
    public partial class MusicPage : UserControl
    {
        private const double DEFAULT_VOLUME = 0.5;

        private SongController _songController;
        private MelodyService _melodyService = new MelodyService();
        bool initialized;

        bool playing;

        public MusicPage(SongController controller)
        {
            InitializeComponent();
            _songController = controller;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!initialized)
            {
                initialized = true;
                VolumeSlider.Value = DEFAULT_VOLUME * 100;

                var data = _melodyService.LoadSong();
                FixMissingNotes(data[0]);
                ShowMeasure(data[0]);
                _songController.SetNextMelodyBar(data[0]);

                _songController.BarStarted += _songController_BarStarted;
            }
        }

        private List<NoteCommand> _fixedNotes = new List<NoteCommand>();

        private void FixMissingNotes(List<NoteCommand> measure)
        {
            foreach (var note in measure.Where(n => n.Note == 0))
            {
                note.Note = 48;
                _fixedNotes.Add(note);
            }
        }

        private void ShowMeasure(List<NoteCommand> notes)
        {
            var s = new List<MusicalSymbol>();
            foreach (var note in notes.OrderBy(n => n.Position))
            {
                var t = TranslateNote(note);
                var psamNote = new Note(t.Note, t.Alter, t.Octave, TranslateDuration(note), NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
                if(_fixedNotes.Contains(note))
                {
                    psamNote.MusicalCharacterColor = System.Drawing.Color.Red;
                }
                s.Add(psamNote);
            }

            viewer.Symbols = s;
        }

        private (string Note, int Alter, int Octave) TranslateNote(NoteCommand note)
        {
            var octave = note.Note / 12 - 1;
            switch (note.Note % 12)
            {
                case 0:
                    return ("C", 0, octave);
                case 1:
                    return ("C", 1, octave);
                case 2:
                    return ("D", 0, octave);
                case 3:
                    return ("D", 1, octave);
                case 4:
                    return ("E", 0, octave);
                case 5:
                    return ("F", 0, octave);
                case 6:
                    return ("F", 1, octave);
                case 7:
                    return ("G", 0, octave);
                case 8:
                    return ("G", 1, octave);
                case 9:
                    return ("A", 0, octave);
                case 10:
                    return ("A", 1, octave);
                case 11:
                    return ("B", 0, octave);
                default:
                    throw new Exception("Unreachable code!");
            }
        }

        private MusicalSymbolDuration TranslateDuration(NoteCommand note)
        {
            switch (note.Duration)
            {
                case 1:
                    return MusicalSymbolDuration.Sixteenth;
                case 2:
                    return MusicalSymbolDuration.Eighth;
                case 4:
                    return MusicalSymbolDuration.Quarter;
                case 8:
                    return MusicalSymbolDuration.Half;
                case 16:
                    return MusicalSymbolDuration.Whole;
                default:
                    throw new ArgumentException($"Don't know how to translate {note.Duration}/16ths of a note.");
            }
        }

        private void _songController_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (playing)
            {
                if (e.BarNumber % 4 == 0)
                {
                    

                    //_songController.SetNextMelodyBar(_melody);
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
                // _songController.SetNextMelodyBar(_melody);

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
