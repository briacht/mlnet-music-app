using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ParseIncomingSong
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Measure> allMeasures = LoadJson("C:/Users/adugar/Documents/ML.NET/Manipulate-data/ParseIncomingSong/happy-cowboy.json");
            DataTable measureData = GetNotesInMeasure(allMeasures);
            DataTable finalTable = GetFinalTable(measureData);
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
                        singleNote.Velocity = 125;
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

        public static DataTable GetNotesInMeasure(List<Measure> allMeasures)
        {
            int i = 0;
            int j = 0;
            int k = 1;

            // Create Data Table structure
            DataTable measureData = new DataTable();
            measureData.Columns.AddRange(new DataColumn[]
            {
            new DataColumn("Measure", typeof(int)),
            new DataColumn("Pitch 1", typeof(int)),
            new DataColumn("Pitch 2", typeof(int)),
            new DataColumn("Pitch 3", typeof(int)),
            new DataColumn("Pitch 4", typeof(int)),
            new DataColumn("Pitch 5", typeof(int)),
            new DataColumn("Pitch 6", typeof(int)),
            new DataColumn("Pitch 7", typeof(int)),
            new DataColumn("Pitch 8", typeof(int)),
            });

            while (i < allMeasures.Count)
            {
                DataRow currentRow = measureData.NewRow();

                while (j < allMeasures[i].Notes.Count)
                {
                    currentRow["Measure"] = i;
                    currentRow[k] = int.Parse(allMeasures[i].Notes[j].Note.ToString());
                    k++;
                    j++;
                }

                measureData.Rows.Add(currentRow);
                j = 0;
                k = 1;
                i++;
            }

            return measureData;
        }

        public static DataTable GetFinalTable(DataTable measureData)
        {
            // Create Data Table structure
            DataTable finalTable = new DataTable();
            finalTable.Columns.AddRange(new DataColumn[]
            {
            new DataColumn("Key", typeof(string)),
            new DataColumn("Measure", typeof(string)),
            new DataColumn("60", typeof(int)),
            new DataColumn("61", typeof(int)),
            new DataColumn("62", typeof(int)),
            new DataColumn("63", typeof(int)),
            new DataColumn("64", typeof(int)),
            new DataColumn("65", typeof(int)),
            new DataColumn("66", typeof(int)),
            new DataColumn("67", typeof(int)),
            new DataColumn("68", typeof(int)),
            new DataColumn("69", typeof(int)),
            new DataColumn("70", typeof(int)),
            new DataColumn("71", typeof(int)),
            new DataColumn("72", typeof(int)),
            new DataColumn("73", typeof(int)),
            new DataColumn("74", typeof(int)),
            new DataColumn("75", typeof(int)),
            new DataColumn("76", typeof(int)),
            new DataColumn("77", typeof(int)),
            new DataColumn("78", typeof(int)),
            new DataColumn("79", typeof(int)),
            });

            //incoming data counters
            int i = 0;

            int k = 1;
            int isPresent = 1;
            int notPresent = 0;

            while (i < measureData.Rows.Count)
            {
                k = 1;
                DataRow currentRow = finalTable.NewRow();
                currentRow["Key"] = 0;
                currentRow["Measure"] = measureData.Rows[i]["Measure"].ToString();

                while (k < measureData.Columns.Count && measureData.Rows[i][k] != DBNull.Value)
                {
                        
                    switch (measureData.Rows[i][k])
                    {
                        case 60:
                            {
                                currentRow[2] = isPresent;
                                break;
                            }
                        case 61:
                            {
                                currentRow[3] = isPresent;
                                break;
                            }
                        case 62:
                            {
                                currentRow[4] = isPresent;
                                break;
                            }
                        case 63:
                            {
                                currentRow[5] = isPresent;
                                break;
                            }
                        case 64:
                            {
                                currentRow[6] = isPresent;
                                break;
                            }
                        case 65:
                            {
                                currentRow[7] = isPresent;
                                break;
                            }
                        case 66:
                            {
                                currentRow[8] = isPresent;
                                break;
                            }
                        case 67:
                            {
                                currentRow[9] = isPresent;
                                break;
                            }
                        case 68:
                            {
                                currentRow[10] = isPresent;
                                break;
                            }
                        case 69:
                            {
                                currentRow[11] = isPresent;
                                break;
                            }
                        case 70:
                            {
                                currentRow[12] = isPresent;
                                break;
                            }
                        case 71:
                            {
                                currentRow[13] = isPresent;
                                break;
                            }
                        case 72:
                            {
                                currentRow[14] = isPresent;
                                break;
                            }
                        case 73:
                            {
                                currentRow[15] = isPresent;
                                break;
                            }
                        case 74:
                            {
                                currentRow[16] = isPresent;
                                break;
                            }
                        case 75:
                            {
                                currentRow[17] = isPresent;
                                break;
                            }
                        case 76:
                            {
                                currentRow[18] = isPresent;
                                break;
                            }
                        case 77:
                            {
                                currentRow[19] = isPresent;
                                break;
                            }
                        case 78:
                            {
                                currentRow[20] = isPresent;
                                break;
                            }
                        case 79:
                            {
                                currentRow[21] = isPresent;
                                break;
                            }
                        case null:
                            break;
                        default:
                            Console.WriteLine("This is not a valid note");
                            break;
                    }

                    for (int x = 1; x < currentRow.ItemArray.Length; x++)
                    {
                        if (currentRow.IsNull(x))
                        {
                            currentRow[x] = notPresent;
                        }
                    }

                    k++;
                }
                    
                finalTable.Rows.Add(currentRow);
                i++;
            }

            return finalTable;
        }

    }
    
}
