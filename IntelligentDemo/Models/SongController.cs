using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace IntelligentDemo.Models
{
    public class NoteCommand
    {
        public byte Note { get; set; }
        public byte Velocity { get; set; }
        public byte Position { get; set; }
        public byte Duration { get; set; }
    }

    public class BarStartedEventArgs : EventArgs
    {
        public int BarNumber { get; set; }
    }

    public class SongController : IDisposable
    {
        private static byte BASS_CHANNEL = 0;
        private static byte MELODY_CHANNEL = 1;
        private static byte PERCUSSION_CHANNEL = 9;

        private int _noteCount = 0;
        private MidiWrapper _midi;
        private DispatcherTimer _timer;
        private List<Action<MidiWrapper>>[] _currentBar;
        private List<Action<MidiWrapper>>[] _carryOver;
        private IEnumerable<NoteCommand> _nextBassBar;
        private IEnumerable<NoteCommand> _nextPercussionBar;

        public SongController()
        {
            _midi = new MidiWrapper();
            _midi.SelectInstrument(BASS_CHANNEL, 34);
            _midi.SelectInstrument(MELODY_CHANNEL, 81);

            _carryOver = new List<Action<MidiWrapper>>[16];
            for (int i = 0; i < _carryOver.Length; i++)
            {
                _carryOver[i] = new List<Action<MidiWrapper>>();
            }

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(125) /* 1/16 notes @ 120 bpm */ };
            _timer.Tick += (s, e) => OnSixteenthNotes();
        }

        public event EventHandler<BarStartedEventArgs> BarStarted;

        protected virtual void OnBarStarted(BarStartedEventArgs e)
        {
            BarStarted?.Invoke(this, e);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        private List<Action<MidiWrapper>>[] BuildNextBar()
        {
            var commands = new List<Action<MidiWrapper>>[16];
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new List<Action<MidiWrapper>>(_carryOver[i]);
                _carryOver[i].Clear();
            }

            if (_nextBassBar != null)
            {
                foreach (var note in _nextBassBar)
                {
                    commands[note.Position - 1].Add(m => m.NoteOn(BASS_CHANNEL, note.Note, note.Velocity));
                    var off = (note.Position - 1) + note.Duration;
                    if (off < 16)
                    {
                        commands[off].Add(m => m.NoteOff(BASS_CHANNEL, note.Note, note.Velocity));
                    }
                    else
                    {
                        _carryOver[off - 16].Add(m => m.NoteOff(BASS_CHANNEL, note.Note, note.Velocity));
                    }
                }
            }

            if (_nextPercussionBar != null)
            {
                foreach (var note in _nextPercussionBar)
                {
                    // Off commands not needed for percussion
                    commands[note.Position - 1].Add(m => m.NoteOn(PERCUSSION_CHANNEL, note.Note, note.Velocity));
                }
            }

            // Metronome
            commands[0].Add(m => m.NoteOn(PERCUSSION_CHANNEL, 81, 100));
            commands[4].Add(m => m.NoteOn(PERCUSSION_CHANNEL, 80, 40));
            commands[8].Add(m => m.NoteOn(PERCUSSION_CHANNEL, 80, 40));
            commands[12].Add(m => m.NoteOn(PERCUSSION_CHANNEL, 80, 40));

            return commands;
        }

        public void SetNextBassBar(IEnumerable<NoteCommand> notes)
        {
            _nextBassBar = notes;
        }

        public void SetNextPercussionBar(IEnumerable<NoteCommand> notes)
        {
            _nextPercussionBar = notes;
        }

        private void OnSixteenthNotes()
        {
            var noteInBar = _noteCount % 16;

            if (noteInBar == 0)
            {
                _currentBar = BuildNextBar();
            }

            if (_currentBar != null)
            {
                _currentBar[noteInBar].ForEach(m => m(_midi));
            }

            if (noteInBar == 0)
            {
                OnBarStarted(new BarStartedEventArgs { BarNumber = _noteCount / 16 + 1 });
            }

            _noteCount++;
        }

        public void Dispose()
        {
            _midi.Dispose();
        }
    }
}
