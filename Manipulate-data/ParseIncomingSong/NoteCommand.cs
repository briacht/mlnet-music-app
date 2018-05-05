using System;
using System.Collections.Generic;
using System.Text;

namespace ParseIncomingSong
{
    public class NoteCommand
    {
        public int Note { get; set; }
        public float Velocity { get; set; }
        public float Position { get; set; }
        public float Duration { get; set; }
    }

}
