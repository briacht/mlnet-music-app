using IntelligentDemo.Convertors;
using IntelligentDemo.Models;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System.Windows.Controls;

namespace IntelligentDemo.ViewModels
{
    public class MeasureViewModel
    {
        public MeasureViewModel(MusicMeasure measure)
        {
            Measure = measure;
            Width = CalculateWidth(measure);

            var grid = new Grid { Width = Width, Height = 100 };
            grid.Children.Add(BuildVisualMeasure(measure));
            DisplayGrid = grid;
        }

        public MusicMeasure Measure { get; set; }
        public Grid DisplayGrid { get; set; }
        public double Width { get; set; }

        public static IncipitViewerWPF BuildClef(uint beats, uint beatType)
        {
            var result = new IncipitViewerWPF();
            result.AddMusicalSymbol(new Clef(ClefType.GClef, 2));
            result.AddMusicalSymbol(new TimeSignature(TimeSignatureType.Numbers, beats, beatType));
            return result;
        }

        private static IncipitViewerWPF BuildVisualMeasure(MusicMeasure measure)
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

        private static int CalculateWidth(MusicMeasure measure)
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
    }
}
