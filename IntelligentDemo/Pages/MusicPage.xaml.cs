using IntelligentDemo.Convertors;
using IntelligentDemo.Models;
using IntelligentDemo.Models.Services;
using IntelligentDemo.Services;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
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
        private int? _controllerBarWhenPlayStarted;
        private MeasureInfo[] _measures;
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
                FixMissingNotes(data);
                _measures = CreateDisplayMeasures(data);
                Redraw();

                _songController.BarStarted += _songController_BarStarted;
            }
        }

        private void _songController_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (playing)
            {
                if (!_controllerBarWhenPlayStarted.HasValue)
                {
                    _controllerBarWhenPlayStarted = e.BarNumber;
                }


                var current = (e.BarNumber - _controllerBarWhenPlayStarted.Value) % _measures.Length;
                var next = (current + 1) % _measures.Length;
                var previous = current == 0
                    ? _measures.Length - 1
                    : current - 1;

                _measures[previous].DisplayGrid.Background = Brushes.White;
                _measures[current].DisplayGrid.Background = Brushes.Yellow;

                _songController.SetNextMelodyBar(_measures[next].Measure.Notes);
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
                _songController.SetNextMelodyBar(_measures[0].Measure.Notes);

                playing = true;
                PlayButton.Background = new SolidColorBrush(Color.FromRgb(0x10, 0x7c, 0x10));

                _songController.Start();
            }
            else
            {
                playing = false;
                _songController.SetNextMelodyBar(null);
                _controllerBarWhenPlayStarted = null;
                PlayButton.Background = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66));

                foreach (var item in _measures)
                {
                    item.DisplayGrid.Background = Brushes.White;
                }
            }
        }

        private void Redraw()
        {
            var columns = 7;
            MusicPanel.Children.Clear();

            StackPanel current = null;
            for (int i = 0; i < _measures.Length; i++)
            {
                if (i % (columns - 1) == 0)
                {
                    current = new StackPanel { Orientation = Orientation.Horizontal };
                    MusicPanel.Children.Add(current);

                    var grid = new Grid { Width = 40, Height = 100 };
                    grid.Children.Add(BuildClef());
                    current.Children.Add(grid);
                }

                current.Children.Add(_measures[i].DisplayGrid);
            }
        }

        private void DetailsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private static MeasureInfo[] CreateDisplayMeasures(IEnumerable<Measure> measures)
        {
            var result = new List<MeasureInfo>();
            foreach (var measure in measures)
            {
                var grid = new Grid { Width = CalculateWidth(measure), Height = 100 };
                grid.Children.Add(BuildVisualMeasure(measure));
                result.Add(new MeasureInfo
                {
                    Measure = measure,
                    DisplayGrid = grid
                });
            }

            return result.ToArray();
        }

        private static IncipitViewerWPF BuildClef()
        {
            var result = new IncipitViewerWPF();
            result.AddMusicalSymbol(new Clef(ClefType.GClef, 2));
            result.AddMusicalSymbol(new TimeSignature(TimeSignatureType.Numbers, 4, 4));
            return result;
        }

        private static IncipitViewerWPF BuildVisualMeasure(Measure measure)
        {
            var result = new IncipitViewerWPF();
            var symbols = MeasureToPsamSymbolConvertor.Convert(measure);
            foreach (var symbol in symbols)
            {
                result.AddMusicalSymbol(symbol);
            }
            result.AddMusicalSymbol(new Barline());

            return result;
        }

        private static int CalculateWidth(Measure measure)
        {
            var width = 0;
            foreach (var note in measure.Notes)
            {
                switch (note.Duration)
                {
                    case 16:
                        width += 50;
                        break;
                    case 8:
                        width += 30;
                        break;
                    case 4:
                        width += 18;
                        break;
                    case 2:
                        width += 15;
                        break;
                    default:
                        width += 14;
                        break;
                }
            }

            // barline
            width += 17;

            return width;
        }

        private static void FixMissingNotes(List<Measure> measures)
        {
            foreach (var measure in measures)
            {
                foreach (var note in measure.Notes.Where(n => n.Note == 0))
                {
                    note.IsPredicted = true;
                    note.Note = 48;
                }
            }
        }
    }

    public class MeasureInfo
    {
        public Measure Measure { get; set; }
        public Grid DisplayGrid { get; set; }
    }
}
