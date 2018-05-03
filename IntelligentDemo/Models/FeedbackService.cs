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
        public async Task<IEnumerable<Feedback>> GetFeedback()
        {
            var result = await LoadFeedback();
            await AnalyzeFeedback(result);
            return result;
        }

        private async Task AnalyzeFeedback(IEnumerable<Feedback> feedback)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI
            {
                AzureRegion = AzureRegions.Westus,
                SubscriptionKey = App.Secrets.SentimentKey
            };

            var inputs = feedback.Select(f => new MultiLanguageInput("en", f.Id, f.Text)).ToList();

            var result = await client.SentimentAsync(new MultiLanguageBatchInput(inputs));

            foreach (var document in result.Documents)
            {
                feedback.Single(f => f.Id == document.Id).Score = document.Score;
            }
        }

        private async Task<IEnumerable<Feedback>> LoadFeedback()
        {
            var data = await Task.Run(() => File.ReadAllText("Feedback.json"));
            var result = JsonConvert.DeserializeObject<Feedback[]>(data);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Id = i.ToString();
            }

            return result;
        }
    }
}
