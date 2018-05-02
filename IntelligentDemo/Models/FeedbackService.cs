using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntelligentDemo.Models
{
    public class FeedbackService
    {
        private Feedback[] _feedback;

        public FeedbackService()
        {
            _feedback = JsonConvert.DeserializeObject<Feedback[]>(File.ReadAllText("Feedback.json"));
            for (int i = 0; i < _feedback.Length; i++)
            {
                _feedback[i].Id = i.ToString();
            }
        }

        public IEnumerable<Feedback> GetFeedback()
        {
            return _feedback;
        }

        public async Task AnalyzeFeedback(IEnumerable<Feedback> feedback)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI
            {
                AzureRegion = AzureRegions.Westus,
                SubscriptionKey = App.Secrets.SentimentKey
            };

            var inputs = _feedback.Select(f => new MultiLanguageInput("en", f.Id, f.Text)).ToList();

            var result = await client.SentimentAsync(new MultiLanguageBatchInput(inputs));

            foreach (var document in result.Documents)
            {
                _feedback.Single(f => f.Id == document.Id).Score = document.Score;
            }
        }
    }
}
