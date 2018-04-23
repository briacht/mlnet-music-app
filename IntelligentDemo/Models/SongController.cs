using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI.Xaml;

namespace IntelligentDemo.Models
{
    public class NoteCommand
    {
        public byte Note { get; set; }
        public byte Velocity { get; set; }
        public byte Position { get; set; }
        public byte Duration { get; set; }
    }

    public class SongController : IDisposable
    {
        private static byte BASS_CHANNEL = 0;
        private static byte MELODY_CHANNEL = 1;

        private DispatcherTimer _timer;
        private IMidiOutPort midiOutPort;
        private int _noteCount = 0;
        private List<IMidiMessage>[] _currentBar;
        private List<IMidiMessage>[] _carryOver;

        private IEnumerable<NoteCommand> _nextBassBar;

        public SongController()
        {
            string midiOutportQueryString = MidiOutPort.GetDeviceSelector();
            DeviceInformationCollection midiOutputDevices = DeviceInformation.FindAllAsync(midiOutportQueryString).AsTask().Result;
            var devInfo = midiOutputDevices.First();
            midiOutPort = MidiOutPort.FromIdAsync(devInfo.Id).AsTask().Result;

            _carryOver = new List<IMidiMessage>[16];
            for (int i = 0; i < _carryOver.Length; i++)
            {
                _carryOver[i] = new List<IMidiMessage>();
            }

            midiOutPort.SendMessage(new MidiProgramChangeMessage(BASS_CHANNEL, 34));
            midiOutPort.SendMessage(new MidiProgramChangeMessage(MELODY_CHANNEL, 81));

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(125) /* 1/16 notes @ 120 bpm */ };
            _timer.Tick += (s, e) => OnSixteenthNotes();
            _timer.Start();
        }

        private List<IMidiMessage>[] BuildNextBar()
        {
            var commands = new List<IMidiMessage>[16];
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new List<IMidiMessage>(_carryOver[i]);
                _carryOver[i].Clear();
            }

            if (_nextBassBar != null)
            {
                foreach (var note in _nextBassBar)
                {
                    commands[note.Position - 1].Add(new MidiNoteOnMessage(BASS_CHANNEL, note.Note, note.Velocity));
                    var off = (note.Position - 1) + note.Duration;
                    if(off < 16)
                    {
                        commands[off].Add(new MidiNoteOffMessage(BASS_CHANNEL, note.Note, note.Velocity));
                    }
                    else
                    {
                        _carryOver[off-16].Add(new MidiNoteOffMessage(BASS_CHANNEL, note.Note, note.Velocity));
                    }
                }
            }

            return commands;
        }

        public void SetNextBassBar(IEnumerable<NoteCommand> notes)
        {
            _nextBassBar = notes;
        }

        public void OnSixteenthNotes()
        {
            var noteInBar = _noteCount % 16;

            if (noteInBar == 0)
            {
                _currentBar = BuildNextBar();
            }

            if (_currentBar != null)
            {
                _currentBar[noteInBar].ForEach(m => midiOutPort.SendMessage(m));
            }

            _noteCount++;
        }

        public void Dispose()
        {
            midiOutPort.Dispose();
        }
    }
}
