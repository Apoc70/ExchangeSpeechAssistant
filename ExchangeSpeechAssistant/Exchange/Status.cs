using AlexaSkillsKit.Speechlet;
using Azure4Alexa.Alexa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Azure4Alexa.Exchange
{
    public class Status
    {

        public static string exchangeStatusUrl =
            "https://ews.mcsmemail.de/ews/exchsnge.asmx";

 
        public static async Task<SpeechletResponse> GetResults(Session session, HttpClient httpClient)
        //public static SpeechletResponse GetResults(Session session, HttpClient httpClient)
        {

            string httpResultString = "";

            // Connect to TFL API Endpoint

            httpClient.DefaultRequestHeaders.Clear();

            //var httpResponseMessage =
            //    httpClient.GetAsync(tflStatusUrl).Result;
            var httpResponseMessage = await httpClient.GetAsync(exchangeStatusUrl);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //httpResultString = httpResponseMessage.Content.ReadAsStringAsync().Result;
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }


            var simpleIntentResponse = ParseResults(httpResultString);
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);

        }

        private static AlexaUtils.SimpleIntentResponse ParseResults(string resultString)
        {
            string stringToRead = String.Empty;
            string stringForCard = String.Empty;

            // you'll need to use JToken instead of JObject with TFL results

            //dynamic resultObject = JToken.Parse(resultString);

            //// if you're into structured data objects, use JArray
            //// JArray resultObject2 = JArray.Parse(resultString);

            //foreach (var i in resultObject)
            //{

            //    if (i.lineStatuses != null)
            //    {
            //        foreach (var j in i.lineStatuses)
            //        {

            //            if (j.disruption != null)
            //            {
            //                stringToRead += "<break time=\"2s\" /> ";
            //                stringToRead += j.disruption.description + " ";
            //                stringForCard += j.disruption.description + " \n\n";
            //            }

            //        }
            //    }
            //}

            // Build the response

            if (stringForCard == String.Empty && stringToRead == String.Empty)
            {
                string noCU = "There is no new Cumulative Update available.";
                stringToRead += Alexa.AlexaUtils.AddSpeakTagsAndClean(noCU);
                stringForCard = noCU;
            }
            else
            {
                stringToRead = Alexa.AlexaUtils.AddSpeakTagsAndClean("There is a new Cumulative Update available.");
                stringForCard = "There is a new Cumulative Update available. \n\n" + stringForCard;
            }

            //return new AlexaUtils.SimpleIntentResponse() { cardText = stringForCard, ssmlString = stringToRead };

            // if you want to add images, you can include them in the reply
            // images should be placed into the ~/Images/ folder of this project
            // 

            // JPEG or PNG supported, no larger than 2MB
            // 720x480 - small size recommendation
            // 1200x800 - large size recommendation

            return new AlexaUtils.SimpleIntentResponse()
            {
                cardText = stringForCard,
                ssmlString = stringToRead,
                largeImage = "msft.png",
                smallImage = "msft.png",
            };

        }
    }
}