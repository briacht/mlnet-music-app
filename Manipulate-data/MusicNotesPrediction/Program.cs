using Microsoft.ML;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;


namespace Microsoft.ML.MusicNotesPrediction
{
    class Program
    {
        // Create paths


        static void Main(string[] args)
        {
            PredictionModel<MusicNotes, MusicNotesPrediction> model = Train();
            Predict(model);
            Evaluate(model);
        }

        public static PredictionModel<MusicNotes, MusicNotesPrediction> Train()
        {
            // Create pipeline

            // Upload data

            // Label Encoder

            // Concatenate columns
            
            // Add learner

            // Convert to note name

            // Train model

            // Write model to .zip

            return model;
        }

        public static void Predict(PredictionModel<MusicNotes, MusicNotesPrediction> model)
        {
            // Add test case


            Console.WriteLine("\n--------------Predicting Test Case--------------");

            // Predict

            Console.WriteLine("Note is " + prediction.Note);
        }

        public static void Evaluate(PredictionModel<MusicNotes, MusicNotesPrediction> model)
        {
            Console.WriteLine("\n--------------Evaluating Model--------------");

            // Load test data

            // Choose evaluator

            // Evaluate

            // Output accuracy


            Console.ReadLine();
        }
    }
}
