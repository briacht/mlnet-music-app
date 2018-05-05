using System;
using System.Collections.Generic;
using System.Text;

namespace ParseIncomingSong
{
    public class Measure
    {
        public IEnumerable<NoteCommand> Notes { get; set; }
    }
}
