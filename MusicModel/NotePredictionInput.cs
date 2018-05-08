using Microsoft.ML.Runtime.Api;

namespace MusicModel
{
    public class NotePredictionInput
    {
        // Column 0 & 1 are not needed for prediction

        [Column("2")]
        public float NoteNumber;
        [Column("3")]
        public float KeySignature;
        [Column("4")]
        public float Note0_Present;
        [Column("5")]
        public float Note1_Present;
        [Column("6")]
        public float Note2_Present;
        [Column("7")]
        public float Note3_Present;
        [Column("8")]
        public float Note4_Present;
        [Column("9")]
        public float Note5_Present;
        [Column("10")]
        public float Note6_Present;
        [Column("11")]
        public float Note7_Present;
        [Column("12")]
        public float Note8_Present;
        [Column("13")]
        public float Note9_Present;
        [Column("14")]
        public float Note10_Present;
        [Column("15")]
        public float Note11_Present;
    }
}
