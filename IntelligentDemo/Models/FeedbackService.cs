using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace IntelligentDemo.Models
{
    public class FeedbackService
    {
        private Feedback[] _feedback;

        public FeedbackService()
        {
            _feedback = JsonConvert.DeserializeObject<Feedback[]>(File.ReadAllText(@"Assets\Feedback.json"));
            for (int i = 0; i < _feedback.Length; i++)
            {
                _feedback[i].Id = i.ToString();
            }

            //ITextAnalyticsAPI client = new TextAnalyticsAPI();
            //client.AzureRegion = AzureRegions.Westus;
            //client.SubscriptionKey = // TODO Read key from somewhere safe;

            //var inputs = _feedback.Select(f => new MultiLanguageInput("en", f.Id, f.Text)).ToList();

            //var result = client.Sentiment(new MultiLanguageBatchInput(inputs));

            //foreach (var document in result.Documents)
            //{
            //    _feedback.Single(f => f.Id == document.Id).Score = document.Score;
            //}
        }

        public IEnumerable<Feedback> GetFeedback()
        {
            return _feedback;
        }

        public class Feedback
        {
            private double? _score;

            public string Id { get; set; }
            public string URL { get; set; }
            public string Image { get; set; }
            public string Text { get; set; }

            public double? Score
            {
                get { return _score; }
                set
                {
                    _score = value;
                    if (value == null)
                    {
                        DisplayScore = null;
                        DisplayColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    }
                    else
                    {
                        DisplayScore = $"Sentiment: {Score.Value:P0}";
                        DisplayColor = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(255 * (1 - value.Value)), Convert.ToByte(255 * value.Value), 0));
                    }
                }
            }

            public string DisplayScore { get; set; }
            public Brush DisplayColor { get; set; }
        }
    }
}
