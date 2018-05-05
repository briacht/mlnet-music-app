using System;
using System.Collections.Generic;
using System.Text;

namespace ParseIncomingSong
{

    public class Rootobject
    {
        public Header header { get; set; }
        public int startTime { get; set; }
        public float duration { get; set; }
        public Track[] tracks { get; set; }
    }

    public class Header
    {
        public int PPQ { get; set; }
        public int bpm { get; set; }
        public int[] timeSignature { get; set; }
        public string name { get; set; }
    }

    public class Track
    {
        public int startTime { get; set; }
        public float duration { get; set; }
        public int length { get; set; }
        public Note[] notes { get; set; }
        public Controlchanges controlChanges { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int instrumentNumber { get; set; }
        public string instrument { get; set; }
        public string instrumentFamily { get; set; }
        public int channelNumber { get; set; }
        public bool isPercussion { get; set; }
    }

    public class Controlchanges
    {
        public _7[] _7 { get; set; }
        public _10[] _10 { get; set; }
        public _32[] _32 { get; set; }
        public _91[] _91 { get; set; }
        public _121[] _121 { get; set; }
    }

    public class _7
    {
        public int number { get; set; }
        public int time { get; set; }
        public float value { get; set; }
    }

    public class _10
    {
        public int number { get; set; }
        public int time { get; set; }
        public float value { get; set; }
    }

    public class _32
    {
        public int number { get; set; }
        public int time { get; set; }
        public int value { get; set; }
    }

    public class _91
    {
        public int number { get; set; }
        public int time { get; set; }
        public float value { get; set; }
    }

    public class _121
    {
        public int number { get; set; }
        public int time { get; set; }
        public int value { get; set; }
    }

    public class Note
    {
        public string name { get; set; }
        public int midi { get; set; }
        public float time { get; set; }
        public float velocity { get; set; }
        public float duration { get; set; }
    }


}
