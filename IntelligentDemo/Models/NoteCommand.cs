namespace IntelligentDemo.Models
{
    public class NoteCommand
    {
        public byte Note { get; set; }
        public byte Velocity { get; set; }
        public byte Position { get; set; }
        public byte Duration { get; set; }
    }
}
