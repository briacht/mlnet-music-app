using IntelligentDemo.Models;
using Microsoft.ML;
using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntelligentDemo.Services
{
    public class MusicRepairer
    {
        private PredictionModel<MusicNotes, MusicNotesPrediction> _model;

        public MusicRepairer()
        {
            _model = PredictionModel.ReadAsync<MusicNotes, MusicNotesPrediction>("Services/MusicModel.zip").Result;
        }

        public void Repair(List<MusicMeasure> measures)
        {
            foreach (var measure in measures)
            {
                var knownNotes = measure.Notes.Where(n => n.Note != 0).Select(n => n.Note);
                var feature = BuildFeature(knownNotes);

                foreach (var note in measure.Notes.Where(n => n.Note == 0))
                {
                    var newNote = _model.Predict(feature).Note;
                    note.Note = Convert(newNote);
                    note.IsRepaired = true;
                }
            }
        }

        private MusicNotes BuildFeature(IEnumerable<byte> knownNotes)
        {
            return new MusicNotes
            {
                Chorale = 1,
                Key = 1,
                N_60 = knownNotes.Contains((byte)60) ? 1 : 0,
                N_61 = knownNotes.Contains((byte)61) ? 1 : 0,
                N_62 = knownNotes.Contains((byte)62) ? 1 : 0,
                N_63 = knownNotes.Contains((byte)63) ? 1 : 0,
                N_64 = knownNotes.Contains((byte)64) ? 1 : 0,
                N_65=  knownNotes.Contains((byte)65) ? 1 : 0,
                N_66 = knownNotes.Contains((byte)66) ? 1 : 0,
                N_67 = knownNotes.Contains((byte)67) ? 1 : 0,
                N_68 = knownNotes.Contains((byte)68) ? 1 : 0,
                N_69 = knownNotes.Contains((byte)69) ? 1 : 0,
                N_70 = knownNotes.Contains((byte)70) ? 1 : 0,
                N_71 = knownNotes.Contains((byte)71) ? 1 : 0,
                N_72 = knownNotes.Contains((byte)72) ? 1 : 0,
                N_73 = knownNotes.Contains((byte)73) ? 1 : 0,
                N_74 = knownNotes.Contains((byte)74) ? 1 : 0,
                N_75 = knownNotes.Contains((byte)75) ? 1 : 0,
                N_76 = knownNotes.Contains((byte)76) ? 1 : 0,
                N_77 = knownNotes.Contains((byte)77) ? 1 : 0,
                N_78 = knownNotes.Contains((byte)78) ? 1 : 0,
                N_79 = knownNotes.Contains((byte)79) ? 1 : 0
            };
        }

        private byte Convert(string note)
        {
            switch (note)
            {
                case "C":
                    return 60;
                case "C#":
                    return 61;
                case "D":
                    return 62;
                case "D#":
                    return 63;
                case "E":
                    return 64;
                case "F":
                    return 65;
                case "F#":
                    return 66;
                case "G":
                    return 67;
                case "G#":
                    return 68;
                case "A":
                    return 69;
                case "A#":
                    return 70;
                case "B":
                    return 71;
                default:
                    throw new ArgumentException();
            }
        }

        class MusicNotes
        {
            public float Chorale;
            public float Key;
            public float Measure;
            public string Note;
            public float N_60;
            public float N_61;
            public float N_62;
            public float N_63;
            public float N_64;
            public float N_65;
            public float N_66;
            public float N_67;
            public float N_68;
            public float N_69;
            public float N_70;
            public float N_71;
            public float N_72;
            public float N_73;
            public float N_74;
            public float N_75;
            public float N_76;
            public float N_77;
            public float N_78;
            public float N_79;

            [ColumnName("Label")]
            public string Label;
        }

        public class MusicNotesPrediction
        {
            [ColumnName("PredictedLabel")]
            public string Note;
        }
    }
}
