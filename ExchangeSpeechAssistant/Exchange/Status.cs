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


            var simpleIntentResponse = ParseResults(httpResultString, "CU");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);

        }

        public static async Task<SpeechletResponse> GetCeoResults(Session session, HttpClient httpClient)
        {
            string httpResultString = "";

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


            var simpleIntentResponse = ParseResults(httpResultString, "CEO");
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

        private static AlexaUtils.SimpleIntentResponse ParseResults(string resultString, string typeString)
        {
            string stringToRead = String.Empty;
            string stringForCard = String.Empty;



            if (stringForCard == String.Empty && stringToRead == String.Empty)
            {
                switch (typeString)
                {

                    case ("ceo"):
                        string ceo = "The CEO mailbox is in good shape. As always.";
                        stringToRead += Alexa.AlexaUtils.AddSpeakTagsAndClean(ceo);
                        stringForCard = ceo;
                        break;
                    case ("cu"):
                        string noCU = "There is no new Cumulative Update available.";
                        stringToRead += Alexa.AlexaUtils.AddSpeakTagsAndClean(noCU);
                        stringForCard = noCU;
                        break;
                    default:
                        string nothingToDo = "This must have been a April fools day thing";
                        stringToRead += Alexa.AlexaUtils.AddSpeakTagsAndClean(nothingToDo);
                        stringForCard = nothingToDo;
                        break;
                }
            }
            else
            {
                stringToRead = Alexa.AlexaUtils.AddSpeakTagsAndClean("Nothing to do here.");
                stringForCard = "Nothing to do here. \n\n" + stringForCard;
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