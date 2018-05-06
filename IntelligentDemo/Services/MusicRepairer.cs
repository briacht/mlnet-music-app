using IntelligentDemo.Models;
using System.Collections.Generic;
using System.Linq;

namespace IntelligentDemo.Services
{
    public class MusicRepairer
    {
        public void Repair(List<MusicMeasure> measures)
        {
            foreach (var measure in measures)
            {
                foreach (var note in measure.Notes.Where(n => n.Note == 0))
                {
                    note.IsRepaired = true;
                    note.Note = 48;
                }
            }
        }
    }
}
