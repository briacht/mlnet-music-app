using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParseIncomingSong
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Measure> allMeasures = LoadJson("C:/Users/adugar/Documents/ML.NET/Manipulate-data/ParseIncomingSong/happy-cowboy.json");

        }

        public static List<Measure> LoadJson(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                var midiData = JsonConvert.DeserializeObject<Rootobject[]>(json);
                var notes = midiData[0].tracks[1].notes;

                int measureCounter = 2;
                int i = 0;

                List<Measure> allMeasures = new List<Measure>();
                
                List<NoteCommand> tempMeasure = new List<NoteCommand>();

                while (i < notes.Length)
                {
                    float time = notes[i].time;
                    

                    if (time < measureCounter)
                    {
                        NoteCommand singleNote = new NoteCommand();

                        singleNote.Note = notes[i].midi;
                        singleNote.Velocity = notes[i].velocity;
                        singleNote.Position = notes[i].time;
                        singleNote.Duration = notes[i].duration;

                        tempMeasure.Add(singleNote);

                        i++;
                    }

                    else
                    {
                        Measure measure = new Measure();
                        measure.Notes = tempMeasure;
                        allMeasures.Add(measure);

                        tempMeasure = new List<NoteCommand>();
                        measureCounter = measureCounter + 2;
                    }

                }

                return allMeasures;

            }

        }
        
    }
    
}
