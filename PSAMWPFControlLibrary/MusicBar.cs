using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSAMWPFControlLibrary
{
    public class MusicBar : IncipitViewerWPF
    {
        public MusicBar()
        {
            AddMusicalSymbol(new Clef(ClefType.GClef, 2));
        }

        private IEnumerable<MusicalSymbol> _symbols;

        public IEnumerable<MusicalSymbol> Symbols
        {
            get
            {
                return _symbols;
            }
            set
            {
                _symbols = value;
                foreach (var symbol in value)
                {
                    AddMusicalSymbol(symbol);
                }
                AddMusicalSymbol(new Barline());
            }
        }
    }
}
