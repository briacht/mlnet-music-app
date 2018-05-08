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
                new MusicNote{ Note = 41, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 41, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 55, Duration = 8, Velocity = 127, Position = 9},
            };

            result["contempt"] = new List<MusicNote>
            {
                new MusicNote{ Note = 41, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 41, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 55, Duration = 8, Velocity = 127, Position = 9},
            };

            result["disgust"] = new List<MusicNote>
            {
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 1},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 3},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 5},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 7},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 9},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 10},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 11},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 12},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 13},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 14 },
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 15},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 16},
            };

            result["fear"] = new List<MusicNote>
            {
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 1},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 3},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 5},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 7},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 9},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 10},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 11},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 12},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 13},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 14 },
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 15},
                new MusicNote{ Note = 38, Duration = 1, Velocity = 100, Position = 16},
            };

            result["happiness"] = new List<MusicNote>
            {
                new MusicNote{ Note = 76, Duration = 2, Velocity = 127, Position = 1},
                new MusicNote{ Note = 76, Duration = 2, Velocity = 127, Position = 3},
                new MusicNote{ Note = 53, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 76, Duration = 2, Velocity = 127, Position = 9},
                new MusicNote{ Note = 76, Duration = 2, Velocity = 127, Position = 11},
                new MusicNote{ Note = 54, Duration = 4, Velocity = 127, Position = 13},
            };

            result["neutral"] = new List<MusicNote>
            {
                new MusicNote{ Note = 56, Duration = 4, Velocity = 127, Position = 1},
                new MusicNote{ Note = 56, Duration = 4, Velocity = 127, Position = 5},
                new MusicNote{ Note = 56, Duration = 4, Velocity = 127, Position = 9},
                new MusicNote{ Note = 56, Duration = 4, Velocity = 127, Position = 13},
            };

            result["sadness"] = new List<MusicNote>
            {
                new MusicNote{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new MusicNote{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            result["surprise"] = new List<MusicNote>
            {
                new MusicNote{ Note = 40, Duration = 1, Velocity = 100, Position = 1},
                new MusicNote{ Note = 40, Duration = 1, Velocity = 100, Position = 3},
                new MusicNote{ Note = 40, Duration = 1, Velocity = 100, Position = 5},
                new MusicNote{ Note = 40, Duration = 1, Velocity = 100, Position = 7},
                new MusicNote{ Note = 55, Duration = 1, Velocity = 100, Position = 9},
            };

            return result;
        }
    }
}
