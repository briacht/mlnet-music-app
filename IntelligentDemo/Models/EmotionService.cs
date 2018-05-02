using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentDemo.Models
{
    class EmotionService
    {
        public async Task<string> DetectEmotion(string file)
        {
            var subscriptionKey = App.Secrets.EmotionKey;
            var uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string uri = uriBase + "?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=emotion";

            byte[] byteData = await Task.Run(() => GetImageAsByteArray(file));

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FaceDetectionResult[]>(resultString);

                if (result.Any())
                {
                    return result.First().FaceAttributes.Emotion.OrderByDescending(t => t.Value).First().Key;
                }
                else
                {
                    return "Did not detect a face";
                }
            }
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        public class FaceDetectionResult
        {
            public String FaceId { get; set; }
            public Attributes FaceAttributes { get; set; }

            public class Attributes
            {
                public Emotions Emotion { get; set; }
            }

            public class Emotions : Dictionary<string, decimal>
            {
                public decimal Anger { get { return this["Anger"]; } set { this["Anger"] = value; } }
                public decimal Contempt { get { return this["Contempt"]; } set { this["Contempt"] = value; } }
                public decimal Disgust { get { return this["Disgust"]; } set { this["Disgust"] = value; } }
                public decimal Fear { get { return this["Fear"]; } set { this["Fear"] = value; } }
                public decimal Happiness { get { return this["Happiness"]; } set { this["Happiness"] = value; } }
                public decimal Neutral { get { return this["Neutral"]; } set { this["Neutral"] = value; } }
                public decimal Sadness { get { return this["Sadness"]; } set { this["Sadness"] = value; } }
                public decimal Surprise { get { return this["Surprise"]; } set { this["Surprise"] = value; } }
            }
        }
    }
}
