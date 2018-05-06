using System.Collections.Generic;

namespace IntelligentDemo.Models.Services
{
    public class MelodyGenerator
    {
        public List<MusicMeasure> GetMelody()
        {
            var bar1 = new List<MusicNote>
                    {
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new MusicNote{ Note = 0, Duration = 2, Velocity = 127, Position = 5},
                        new MusicNote{ Note = 76, Duration = 2, Velocity = 127, Position = 7},
                        new MusicNote{ Note = 0, Duration = 4, Velocity = 127, Position = 9},
                        new MusicNote{ Note = 74, Duration = 2, Velocity = 127, Position = 13},
                        new MusicNote{ Note = 72, Duration = 2, Velocity = 127, Position = 15},
                    };

            var bar2 = new List<MusicNote>
                    {
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 5},
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 9},
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 13},
                    };

            var bar3 = new List<MusicNote>
                    {
                        new MusicNote{ Note = 0, Duration = 8, Velocity = 127, Position = 1},
                        new MusicNote{ Note = 72, Duration = 8, Velocity = 127, Position = 9},
                    };

            var bar4 = new List<MusicNote>
                    {
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 5},
                        new MusicNote{ Note = 72, Duration = 2, Velocity = 127, Position = 9},
                        new MusicNote{ Note = 72, Duration = 3, Velocity = 127, Position = 11},
                        new MusicNote{ Note = 72, Duration = 4, Velocity = 127, Position = 13},
                    };

            return new List<MusicMeasure>
            {
                new MusicMeasure { Notes = bar1 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar3 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar1 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar3 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar1 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar3 },
                new MusicMeasure { Notes = bar2 },
                new MusicMeasure { Notes = bar2 }
            };
        }

    }
}
