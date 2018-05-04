using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentDemo.Models.Music
{
    public class MelodyService
    {
        public List<List<NoteCommand>> LoadSong()
        {
            var bar1 = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 72, Duration = 4, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 74, Duration = 2, Velocity = 127, Position = 5},
                        new NoteCommand{ Note = 76, Duration = 2, Velocity = 127, Position = 7},
                        new NoteCommand{ Note = 0, Duration = 4, Velocity = 127, Position = 9},
                        new NoteCommand{ Note = 74, Duration = 2, Velocity = 127, Position = 13},
                        new NoteCommand{ Note = 72, Duration = 2, Velocity = 127, Position = 15},
                    };

            var bar2 = new List<NoteCommand>
                    {
                        new NoteCommand{ Note = 0, Duration = 8, Velocity = 127, Position = 1},
                        new NoteCommand{ Note = 72, Duration = 8, Velocity = 127, Position = 9},
                    };

            return new List<List<NoteCommand>> { bar1, bar2, bar2, bar1 };
        }

    }
}
