using System.Collections.Generic;
using System.Linq;

namespace IntelligentDemo.Models.Music
{
    public class BassLineGenerator
    {
        private Dictionary<string, IEnumerable<NoteCommand>> _lines = InitializeBassLines();
        private int _count;

        public IEnumerable<NoteCommand> GetBassLine(string emotion, byte volume)
        {
            var line = _lines.ContainsKey(emotion.ToLower())
                ? _lines[emotion.ToLower()]
                : _lines.ElementAt(_count % _lines.Count).Value;

            _count++;

            return volume == 127
                ? line
                : line.Select(n => new NoteCommand { Note = n.Note, Duration = n.Duration, Velocity = volume, Position = n.Position });
        }

        private static Dictionary<string, IEnumerable<NoteCommand>> InitializeBassLines()
        {
            var result = new Dictionary<string, IEnumerable<NoteCommand>>();

            result["anger"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 16, Velocity = 127, Position = 1},
            };

            result["contempt"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 13},
            };

            result["disgust"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            result["fear"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 3},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 7},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 9},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 11},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 13},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 15},
            };

            result["happiness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 7},
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 13},
                new NoteCommand{ Note = 38, Duration = 2, Velocity = 127, Position = 15},
            };

            result["neutral"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            result["sadness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            result["surprise"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 13},
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 15},
            };

            return result;
        }
    }
}
