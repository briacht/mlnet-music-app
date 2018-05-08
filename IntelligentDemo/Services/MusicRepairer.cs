using IntelligentDemo.Models;
using Microsoft.ML;
using MusicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntelligentDemo.Services
{
    public class MusicRepairer
    {
        private string _modelPath;

        public MusicRepairer(string modelPath)
        {
            _modelPath = modelPath;
        }

        public async Task Repair(List<MusicMeasure> measures)
        {
            var model = await PredictionModel.ReadAsync<NotePredictionInput, PredictedNote>(_modelPath);

            foreach (var measure in measures)
            {
                foreach (var note in measure.Notes.Where(n => n.Note == 0))
                {
                    var knownNotes = measure.Notes.
                        Where(n => n.Note != 0)
                        .Select(n => n.Note);

                    var feature = BuildFeature(knownNotes);

                    var result = model.Predict(feature);

                    var newNote = AdjustToMeasureOctave(result.NoteNumber, knownNotes);
                    note.Note = newNote;

                    note.IsRepaired = true;
                }
            }
        }

        private NotePredictionInput BuildFeature(IEnumerable<byte> knownNotes)
        {
            var normalized = knownNotes.Select(n => n % 12);

            return new NotePredictionInput
            {
                KeySignature = 0,
                Note0_Present = normalized.Contains(0) ? 1 : 0,
                Note1_Present = normalized.Contains(1) ? 1 : 0,
                Note2_Present = normalized.Contains(2) ? 1 : 0,
                Note3_Present = normalized.Contains(3) ? 1 : 0,
                Note4_Present = normalized.Contains(4) ? 1 : 0,
                Note5_Present = normalized.Contains(5) ? 1 : 0,
                Note6_Present = normalized.Contains(6) ? 1 : 0,
                Note7_Present = normalized.Contains(7) ? 1 : 0,
                Note8_Present = normalized.Contains(8) ? 1 : 0,
                Note9_Present = normalized.Contains(9) ? 1 : 0,
                Note10_Present = normalized.Contains(10) ? 1 : 0,
                Note11_Present = normalized.Contains(11) ? 1 : 0,
            };
        }

        private byte AdjustToMeasureOctave(float note, IEnumerable<byte> knownNotes)
        {
            // Find note within octave that average is in
            var avg = (int)knownNotes.Select(n => Convert.ToInt32(n)).Average();
            var octave = avg / 12;
            var result = octave * 12 + note;

            // Check if the corresponding note in the octave above/below
            // is closer to the average
            if (result - avg > 6)
            {
                result -= 12;
            }
            else if (avg - result > 6)
            {
                result += 12;
            }

            return (byte)result;
        }
    }
}
