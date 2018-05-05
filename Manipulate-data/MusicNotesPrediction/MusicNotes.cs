using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ML.MusicNotesPrediction
{
    // Chorale	Key	Measure	Note	60	61	62	63	64	65	66	67	68	69	70	71	72	73	74	75	76	77	78	79

    class MusicNotes
    {
        public float Chorale;
        public float Key;
        public float Measure;
        public string Note;
        public float N_60;
        public float N_61;
        public float N_62;
        public float N_63;
        public float N_64;
        public float N_65;
        public float N_66;
        public float N_67;
        public float N_68;
        public float N_69;
        public float N_70;
        public float N_71;
        public float N_72;
        public float N_73;
        public float N_74;
        public float N_75;
        public float N_76;
        public float N_77;
        public float N_78;
        public float N_79;

        [ColumnName("Label")]
        public string Label;
    }

    public class MusicNotesPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Note;
    }
}

