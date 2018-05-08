using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawData = "chorales.csv";
            var manipulatedData = "chorales-modified.csv";
            var model = "MusicModel.zip";

            ManipulateData(rawData, manipulatedData);

            // CreateModel(manipulatedData, model).Wait();

            // TestModel(model).Wait();
        }

        static void ManipulateData(string inputPath, string outputPath)
        {
            Console.WriteLine("------ Data Manipulation ------");
            using (var stream = File.OpenWrite(outputPath))
            {
                using (var writer = new StreamWriter(stream))
                {
                    // Headers
                    writer.WriteLine("Chorale,Measure,NoteNumber,KeySignature,Note0_Present,Note1_Present,Note2_Present,Note3_Present,Note4_Present,Note5_Present,Note6_Present,Note7_Present,Note8_Present,Note9_Present,Note10_Present,Note11_Present");

                    var data = File.ReadLines(inputPath).Where(s => !s.StartsWith(",,,"));

                    foreach (var song in data)
                    {
                        var values = song.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                        var chorale = values[0];

                        Debug.Assert(values.Length % 6 == 1, "Expect chorale number then groups of 6 values");

                        var notes = new List<NoteTemp>();
                        for (int i = 1; i < values.Length; i += 6)
                        {
                            Debug.Assert(values[i].StartsWith("st "));
                            var start = Convert.ToInt32(values[i].Replace("st ", ""));

                            Debug.Assert(values[i + 1].StartsWith("pitch "));
                            var note = Convert.ToInt32(values[i + 1].Replace("pitch ", ""));

                            Debug.Assert(values[i + 2].StartsWith("dur "));
                            var duration = Convert.ToInt32(values[i + 2].Replace("dur ", ""));

                            Debug.Assert(values[i + 3].StartsWith("keysig "));
                            var keySignature = Convert.ToInt32(values[i + 3].Replace("keysig ", ""));

                            Debug.Assert(values[i + 4].StartsWith("timesig "));
                            var timeSignature = Convert.ToInt32(values[i + 4].Replace("timesig ", ""));

                            notes.Add(new NoteTemp
                            {
                                Note = note % 12,
                                KeySignature = keySignature,
                                Measure = start / timeSignature
                            });
                        }

                        var measures = notes.GroupBy(n => n.Measure);
                        foreach (var measure in measures)
                        {
                            foreach (var note in measure)
                            {
                                var otherNotes = measure.Where(n => n != note).Select(n => n.Note);

                                object[] f = new object[16];
                                f[0] = chorale;
                                f[1] = note.Measure;
                                f[2] = note.Note;
                                f[3] = note.KeySignature;

                                for (int i = 0; i < 12; i++)
                                {
                                    f[i + 4] = otherNotes.Contains(i) ? 1 : 0;
                                }

                                writer.WriteLine(string.Join(",", f));
                            }
                        }
                    }
                }
            }

            Console.WriteLine("done");
        }

        static async Task CreateModel(string dataPath, string modelPath)
        {
            Console.WriteLine();
            Console.WriteLine("------ Model Creation ------");
            
            // TODO Create model














        }

        static async Task TestModel(string modelPath)
        {
            Console.WriteLine();
            Console.WriteLine("------ Model Testing ------");
            var model = await PredictionModel.ReadAsync<NotePredictionInput, PredictedNote>(modelPath);

            var input = new NotePredictionInput
            {
                KeySignature = 1,
                Note8_Present = 1,
                Note11_Present = 1
            };

            // TODO Predict missing note
            Console.WriteLine($"Predicted ??");
        }
    }
}
