using Microsoft.ML.Runtime.Api;

namespace MusicModel
{
    public class PredictedNote
    {
        [ColumnName("PredictedLabel")]
        public float NoteNumber;
    }
}
