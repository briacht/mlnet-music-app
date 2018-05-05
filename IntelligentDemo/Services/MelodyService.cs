using System.Collections.Generic;

namespace IntelligentDemo.Models.Services
{
    public class MelodyService
    {
        public List<Measure> LoadSong()
        {
            var bar1 = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 0, Duration = 2, Velocity = 127, Position = 5},
                        new NoteCommand{ Note = 76, Duration = 2, Velocity = 127, Position = 7},
                        new NoteCommand{ Note = 0, Duration = 4, Velocity = 127, Position = 9},
                        new NoteCommand{ Note = 74, Duration = 2, Velocity = 127, Position = 13},
                        new NoteCommand{ Note = 72, Duration = 2, Velocity = 127, Position = 15},
                    };

            var bar2 = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 5},
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 9},
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 13},
                    };

            var bar3 = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 0, Duration = 8, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 72, Duration = 8, Velocity = 127, Position = 9},
                    };

            var bar4 = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 5},
                        new NoteCommand{ Note = 72, Duration = 2, Velocity = 127, Position = 9},
                        new NoteCommand{ Note = 72, Duration = 3, Velocity = 127, Position = 11},
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 13},
                    };

            return new List<Measure>
            {
                new Measure { Notes = bar1 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar3 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar1 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar3 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar1 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar3 },
                new Measure { Notes = bar2 },
                new Measure { Notes = bar2 }
            };
        }

    }
}
