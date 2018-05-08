using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Manipulate_data
{
    public class NoteData
    {
        public int NoteNumber { get; set; }
        public int KeySignature { get; set; }
        public int Note0_Present { get; set; }
        public int Note1_Present { get; set; }
        public int Note2_Present { get; set; }
        public int Note3_Present { get; set; }
        public int Note4_Present { get; set; }
        public int Note5_Present { get; set; }
        public int Note6_Present { get; set; }
        public int Note7_Present { get; set; }
        public int Note8_Present { get; set; }
        public int Note9_Present { get; set; }
        public int Note10_Present { get; set; }
        public int Note11_Present { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var inputPath = "chorales.csv";
            var outputPath = "chorales-modified.csv";

            var data = File.ReadLines(inputPath)
                .Where(s => !s.StartsWith(",,,"));

            var result = new List<NoteData>();
            foreach (var song in data)
            {
                var values = song.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                var chorale = values[0];

                var notes = new List<NoteTemp>();
                Debug.Assert(values.Length % 6 == 1, "Expect groups of 8 values plus song number");
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
                        Measure = start % timeSignature
                    });
                }

                var measures = notes.GroupBy(n => n.Measure);
                foreach (var measure in measures)
                {
                    foreach (var note in measure)
                    {
                        var otherNotes = measure.Where(n => n != note).Select(n => n.Note);
                        result.Add(new NoteData
                        {
                            NoteNumber = note.Note,
                            KeySignature = note.KeySignature,
                            Note0_Present = measure.Any(n => n.Note == 0) ? 1 : 0,
                            Note1_Present = measure.Any(n => n.Note == 1) ? 1 : 0,
                            Note2_Present = measure.Any(n => n.Note == 2) ? 1 : 0,
                            Note3_Present = measure.Any(n => n.Note == 3) ? 1 : 0,
                            Note4_Present = measure.Any(n => n.Note == 4) ? 1 : 0,
                            Note5_Present = measure.Any(n => n.Note == 5) ? 1 : 0,
                            Note6_Present = measure.Any(n => n.Note == 6) ? 1 : 0,
                            Note7_Present = measure.Any(n => n.Note == 7) ? 1 : 0,
                            Note8_Present = measure.Any(n => n.Note == 8) ? 1 : 0,
                            Note9_Present = measure.Any(n => n.Note == 9) ? 1 : 0,
                            Note10_Present = measure.Any(n => n.Note == 10) ? 1 : 0,
                            Note11_Present = measure.Any(n => n.Note == 11) ? 1 : 0,
                        });
                    }
                }
            }


            //List<string> rawData = ReadInCSV(inputPath);
            //List<string> cleanData = CleanData(rawData);
            //DataTable noteData = SplitChorales(cleanData);
            //DataTable measureData = ConsolidateMeasureData(noteData);
            //DataTable finalTable = GetFinalTable(measureData);
            //ProduceFinalCSV(finalTable, outputPath);
        }

        public class NoteTemp
        {
            public int Measure { get; set; }
            public int Note { get; set; }
            public int KeySignature { get; set; }
        }

        public static List<string> ReadInCSV(string absolutePath)
        {
            List<string> result = new List<string>();
            string value;
            using (TextReader fileReader = File.OpenText(absolutePath))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                while (csv.Read())
                {
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                    {
                        result.Add(value);
                    }
                }
            }
            return result;
        }

        public static List<string> CleanData(List<string> messyData)
        {
            var cleanData = new List<string>();

            foreach (var s in messyData)
            {
                if (!string.IsNullOrEmpty(s))
                    cleanData.Add(s);
            }

            return cleanData;
        }

        public static DataTable SplitChorales(List<string> data)
        {
            int value = 0;
            int chorale = 0;
            int i = 0;
            int j = 0;

            // Create Data Table structure
            DataTable noteData = new DataTable();
            noteData.Columns.AddRange(new DataColumn[]
            {
            new DataColumn("Chorale", typeof(int)),
            new DataColumn("Start", typeof(int)),
            new DataColumn("Pitch", typeof(int)),
            new DataColumn("Duration", typeof(int)),
            new DataColumn("Key Signature", typeof(int)),
            new DataColumn("Time Signature", typeof(int)),
            new DataColumn("Fermata", typeof(int)),
            });

            DataRow currentRow = noteData.NewRow();


            while (i < data.Count)
            {
                while (j <= 6)
                {
                    if (int.TryParse(data[i], out value))
                    {
                        currentRow = noteData.NewRow();
                        chorale = value;
                        currentRow[j] = chorale;
                        i++;
                        j++;
                    }
                    else if (j == 0)
                    {
                        currentRow = noteData.NewRow();
                        currentRow[j] = chorale;
                        j++;
                    }
                    else
                    {
                        string input = Regex.Match(data[i], @"\d+").Value;
                        int numberOnly = int.Parse(input);
                        currentRow[j] = numberOnly;
                        i++;
                        j++;
                    }

                }

                noteData.Rows.Add(currentRow);
                j = 0;
            }

            return noteData;
        }

        public static DataTable ConsolidateMeasureData(DataTable noteData)
        {
            int choraleCurrent = 1;
            int choraleNext = 1;
            int durationCounter = 0;
            int timeSignature = 0;
            int keySignature = 0;
            int measureCounter = 1;
            int i = 0;
            int j = 3;

            // Create Data Table structure
            DataTable measureData = new DataTable();
            measureData.Columns.AddRange(new DataColumn[]
            {
            new DataColumn("Chorale", typeof(int)),
            new DataColumn("Key", typeof(int)),
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
            DataRow currentRow = measureData.NewRow();

            while (i < noteData.Rows.Count)
            {
                while(choraleNext == choraleCurrent && i < noteData.Rows.Count)
                {
                    choraleNext = int.Parse(noteData.Rows[i]["Chorale"].ToString());
                    keySignature = int.Parse(noteData.Rows[i]["Key Signature"].ToString());
                    timeSignature = int.Parse(noteData.Rows[i]["Time Signature"].ToString());
                    currentRow = measureData.NewRow();

                    while (durationCounter < timeSignature && i < noteData.Rows.Count)
                    {
                        durationCounter = durationCounter + int.Parse(noteData.Rows[i]["Duration"].ToString());
                        currentRow["Measure"] = measureCounter;
                        currentRow[j] = int.Parse(noteData.Rows[i]["Pitch"].ToString());
                        j++;
                        i++;
                    }

                    currentRow["Chorale"] = choraleCurrent;
                    currentRow["Key"] = keySignature;
                    measureData.Rows.Add(currentRow);
                    measureCounter++;
                    durationCounter = 0;
                    j = 3;
                }

                measureCounter = 1;
                choraleCurrent = choraleNext;
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
            new DataColumn("Chorale", typeof(string)),
            new DataColumn("Key", typeof(string)),
            new DataColumn("Measure", typeof(string)),
            new DataColumn("Note", typeof(string)),
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
            int j = 3;
            int k = 3;
            int isPresent = 1;
            int notPresent = 0;
            int visitCounter = 0;

            while (i < measureData.Rows.Count)
            {
                j = 3;

                while (j < measureData.Columns.Count && measureData.Rows[i][j] != DBNull.Value)
                {
                    k = 3;
                    DataRow currentRow = finalTable.NewRow();
                    currentRow["Chorale"] = measureData.Rows[i]["Chorale"].ToString();
                    currentRow["Key"] = measureData.Rows[i]["Key"].ToString();
                    currentRow["Measure"] = measureData.Rows[i]["Measure"].ToString();
                    currentRow["Note"] = int.Parse(measureData.Rows[i][j].ToString());

                    while (k < measureData.Columns.Count && measureData.Rows[i][k] != DBNull.Value)
                    {
                        if (measureData.Rows[i][k].ToString() == currentRow[3].ToString())
                        {
                            visitCounter++;
                        }
                        else
                        {
                            visitCounter = 0;
                        }
                            
                        switch (measureData.Rows[i][k])
                        {
                            case 60:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[4] = isPresent;
                                    break;
                                }
                            case 61:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[5] = isPresent;
                                    break;
                                }
                            case 62:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[6] = isPresent;
                                    break;
                                }
                            case 63:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[7] = isPresent;
                                    break;
                                }
                            case 64:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[8] = isPresent;
                                    break;
                                }
                            case 65:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[9] = isPresent;
                                    break;
                                }
                            case 66:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[10] = isPresent;
                                    break;
                                }
                            case 67:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                 {
                                    currentRow[11] = isPresent;
                                    break;
                                }
                            case 68:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[12] = isPresent;
                                    break;
                                }
                            case 69:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[13] = isPresent;
                                    break;
                                }
                            case 70:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[14] = isPresent;
                                    break;
                                }
                            case 71:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[15] = isPresent;
                                    break;
                                }
                            case 72:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[16] = isPresent;
                                    break;
                                }
                            case 73:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[17] = isPresent;
                                    break;
                                }
                            case 74:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[18] = isPresent;
                                    break;
                                }
                            case 75:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[19] = isPresent;
                                    break;
                                }
                            case 76:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[20] = isPresent;
                                    break;
                                }
                            case 77:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[21] = isPresent;
                                    break;
                                }
                            case 78:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[22] = isPresent;
                                    break;
                                }
                            case 79:
                                if (visitCounter == 1)
                                {
                                    break;
                                }
                                else
                                {
                                    currentRow[23] = isPresent;
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

                    visitCounter = 0;
                    j++;
                    currentRow[3] = GetNoteFromNumber(int.Parse(currentRow[3].ToString()));
                    finalTable.Rows.Add(currentRow);
                    
                }

                i++;
            }

            return finalTable;
        }

        public static void AddBeforeAfterMeasures(DataTable measureData)
        {
            int choraleCurrent = 1;
            int choraleNext = 1;
        }

        public static void ProduceFinalCSV(DataTable finalTable, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = finalTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in finalTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public static string GetNoteFromNumber(int noteNumber)
        {
            string note = "";

            switch (noteNumber)
            {
                case 60:
                    note = "C";
                    break;
                case 61:
                    note = "C#";
                    break;
                case 62:
                    note = "D";
                    break;
                case 63:
                    note = "D#";
                    break;
                case 64:
                    note = "E";
                    break;
                case 65:
                    note = "F";
                    break;
                case 66:
                    note = "F#";
                    break;
                case 67:
                    note = "G";
                    break;
                case 68:
                    note = "G#";
                    break;
                case 69:
                    note = "A";
                    break;
                case 70:
                    note = "A#";
                    break;
                case 71:
                    note = "B";
                    break;
                case 72:
                    note = "C";
                    break;
                case 73:
                    note = "C#";
                    break;
                case 74:
                    note = "D";
                    break;
                case 75:
                    note = "D#";
                    break;
                case 76:
                    note = "E";
                    break;
                case 77:
                    note = "F";
                    break;
                case 78:
                    note = "F#";
                    break;
                case 79:
                    note = "G";
                    break;
                default:
                    Console.WriteLine("This is not a valid note number");
                    break;
            }

            return note;
        }

        public static string[,] GetMinMax(DataTable noteData)
        {
            string[,] minMax = new string[1,2];

            var minPitchRow = noteData.Select("Pitch = MIN(Pitch)");
            var minPitch = minPitchRow[0]["Pitch"].ToString();

            var maxPitchRow = noteData.Select("Pitch = MAX(Pitch)");
            var maxPitch = maxPitchRow[0]["Pitch"].ToString();

            minMax[0, 0] = minPitch;
            minMax[0, 1] = maxPitch;

            return minMax;
        }
    }
}
