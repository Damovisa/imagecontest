using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ImageContest.ImageSearch {
    public class ImageSearcher {

        public string SubscriptionKey { get; }
        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/images/search";
        public ImageSearcher(string subscriptionKey)
        {
            SubscriptionKey = subscriptionKey;
        }

        public TwoSearchResults GetImageSearchResults(string searchTerm) {
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchTerm) + "&safeSearch=Strict";
            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = SubscriptionKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            var randomIndex = new Random().Next(1,10);

            return new TwoSearchResults {
                Img1Url = (string)jsonObj["value"][0].thumbnailUrl,
                Img2Url = (string)jsonObj["value"][randomIndex].thumbnailUrl,
                Img2Index = randomIndex
            };
        }
    }

    public class TwoSearchResults {
        public string Img1Url { get; set; }
        public string Img2Url { get; set; }

        // image 1's index will always be 0
        public int Img2Index { get; set; }
    }
}