using IntelligentDemo.Models;
using System.Collections.Generic;
using System.Linq;

namespace IntelligentDemo.Services
{
    public class PercussionGenerator
    {
        private Dictionary<string, IEnumerable<MusicNote>> _lines = InitializePercussionLines();
        private int _count;

        public IEnumerable<MusicNote> GetPercussionMeasure(string emotion)
        {
            var line = _lines.ContainsKey(emotion.ToLower())
                ? _lines[emotion.ToLower()]
                : _lines.ElementAt(_count % _lines.Count).Value;

            _count++;

            return line;
        }

        private static Dictionary<string, IEnumerable<MusicNote>> InitializePercussionLines()
        {
            var result = new Dictionary<string, IEnumerable<MusicNote>>();

            result["anger"] = new List<MusicNote>
            {
                new MusicNote{ Note = 36, Duration = 16, Velocity = 127, Position = 1},
            };

            result["contempt"] = new List<MusicNote>
            {
                new MusicNote{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 36, Duration = 4, Velocity = 100, Position = 5},
                new MusicNote{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new MusicNote{ Note = 36, Duration = 4, Velocity = 100, Position = 13},
            };

            result["disgust"] = new List<MusicNote>
            {
                new MusicNote{ Note = 40, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            result["fear"] = new List<MusicNote>
            {
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 1},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 3},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 5},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 7},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 9},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 11},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 13},
                new MusicNote{ Note = 48, Duration = 2, Velocity = 100, Position = 15},
            };

            result["happiness"] = new List<MusicNote>
            {
                new MusicNote{ Note = 48, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 50, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 48, Duration = 4, Velocity = 127, Position = 9},
                new MusicNote{ Note = 50, Duration = 4, Velocity = 127, Position = 13},
            };

            result["neutral"] = new List<MusicNote>
            {
                new MusicNote{ Note = 48, Duration = 8, Velocity = 127, Position = 1},
                new MusicNote{ Note = 48, Duration = 8, Velocity = 127, Position = 9},
            };

            result["sadness"] = new List<MusicNote>
            {
                new MusicNote{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new MusicNote{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            result["surprise"] = new List<MusicNote>
            {
                new MusicNote{ Note = 38, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 60, Duration = 8, Velocity = 127, Position = 9},
            };

            return result;
        }
    }
}
