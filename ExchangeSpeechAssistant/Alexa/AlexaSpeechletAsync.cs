﻿using System;
using System.Collections.Generic;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.UI;
using System.Diagnostics;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Azure4Alexa.Alexa
{
    // Follow the AlexaSkillsKit documentation and override the base class and children with our own implementation
    // We'll implement the web services friendly async variant - SpeechletAsync

    // the functions below map to the Alexa requests described at this URL
    // https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/handling-requests-sent-by-alexa

    public class AlexaSpeechletAsync : SpeechletAsync
    {

        // Alexa provides a security wrapper around requests sent to your service, which the 
        // AlexaSkillsKit nuget package validates by default.  However, you might not want this wrapper enabled while
        // you do local development and testing - in DEBUG mode.

        // Note: the default Azure publishing option in Visual Studio is Release (not Debug), so by default the
        // security wrapper will be enabled when you publish to Azure.

        // Amazon requires that your skill validate requests sent to it for certification, so you shouldn't 
        // deploy to production with validation disabled

        //#if DEBUG

        public override bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc, SpeechletRequestEnvelope requestEnvelope)
        {
            return true;
        }

        //#endif

        public override Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            // this function is invoked when a user begins a session with your skill
            // this is a chance to load user data at the start of a session

            // if the inbound request doesn't include your Alexa Skills AppId or you haven't updated your
            // code to include the correct AppId, return a visual and vocal error and do no more
            // Update the AppId variable in AlexaConstants.cs to resolve this issue

            if (AlexaUtils.IsRequestInvalid(session))
            {
                return Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }

            // to-do - up to you

            // return some sort of Task per function definition
            return Task.Delay(0);
        }

        public override Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            // this function is invoked when a user ends a session with your skill
            // this is a chance to save user data at the end of a session

            // if the inbound request doesn't include your Alexa Skills AppId or you haven't updated your
            // code to include the correct AppId, return a visual and vocal error and do no more
            // Update the AppId variable in AlexaConstants.cs to resolve this issue

            if (AlexaUtils.IsRequestInvalid(session))
            {
                return Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }


            // to-do - up to you

            // return some sort of Task per function definition
            return Task.Delay(0);

        }

        public override async Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            // this function is invoked when the user invokes your skill without an intent

            // if the inbound request doesn't include your Alexa Skills AppId or you haven't updated your
            // code to include the correct AppId, return a visual and vocal error and do no more
            // Update the AppId variable in AlexaConstants.cs to resolve this issue

            if (AlexaUtils.IsRequestInvalid(session))
            {
                return await Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }


            return await Task.FromResult<SpeechletResponse>(GetOnLaunchAsyncResult(session));
        }

        public override async Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)
        //        public override Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)
        {
            // if the inbound request doesn't include your Alexa Skills AppId or you haven't updated your
            // code to include the correct AppId, return a visual and vocal error and do no more
            // Update the AppId variable in AlexaConstants.cs to resolve this issue

            if (AlexaUtils.IsRequestInvalid(session))
            {
                return await Task.FromResult<SpeechletResponse>(InvalidApplicationId(session));
            }

            // this function is invoked when Amazon matches what the user said to 
            // one of your defined intents.  now you will need to handle
            // the request

            // intentRequest.Intent.Name contains the name of the intent
            // intentRequest.Intent.Slots.* contains slot values if you're using them 
            // session.User.AccessToken contains the Oauth 2.0 access token if the user has linked to your auth system

            // Get intent from the request object.
            Intent intent = intentRequest.Intent;
            string intentName = (intent != null) ? intent.Name : null;

            // If there's no match between the intent passed and what we support, (i.e. you forgot to implement
            // a handler for the intent), default the user to the standard OnLaunch request

            // you'll probably be calling a web service to handle your intent
            // this is a good place to create an httpClient that can be recycled across REST API requests
            // don't be evil and create a ton of them unnecessarily, as httpClient doesn't clean up after itself

            var httpClient = new HttpClient();

            switch (intentName)
            {

                case ("ExchangeCuIntent"):
                    return await Exchange.Status.GetResults(session, httpClient);

                case ("ExchangeCeoStatus"):
                    return await Exchange.Status.GetCeoResults(session, httpClient);

                // Open implementations (https://github.com/Apoc70/ExchangeSpeechAssistant/issues/1)

                //case ("ExchangeStatusIntent"):
                //    return await Exchange.Status.GetGeneralStatusResults(session, httpClient);

                //case ("ExchangeCorruptIndexIntent"):
                //    return await Exchange.Status.GetCorruptIndexResults(session, httpClient);

                default:
                    return await Task.FromResult<SpeechletResponse>(GetOnLaunchAsyncResult(session));
            }

        }


        private SpeechletResponse GetOnLaunchAsyncResult(Session session)
        {
            // called by OnLaunchAsync - when the user invokes your skill without an intent
            // called by OnIntentAsync if you forget to map an intent to an action

            return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = "Thanks for giving us a try" }, true);
        }


        private SpeechletResponse InvalidApplicationId(Session session)
        {
            // if the inbound request doesn't include your Alexa Skills AppId or you haven't updated your
            // code to include the correct AppId, return a visual and vocal error and do no more
            // Update the AppId variable in AlexaConstants.cs to resolve this issue

            return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse()
            {
                cardText = "An invalid Application ID was received from Alexa.  Please update your Visual Studio project " +
                    "to include the correct value and then re-deploy your Azure project."
            }, true);
        }

        // handle the enqueuing of the next track in a multi-track album, which can occur after Alexa informs your
        // service (this service) that the current track has begun.

        public override async Task<SpeechletResponse> OnAudioPlayerAsync(AudioPlayerRequest audioPlayerRequest,
            Context context)
        {
            var httpClient = new HttpClient();
            //return await Groove.Music.EnqueueGrooveMusic(context, httpClient, "ENQUEUE");
            return null;
        }

        // handle the user asking for the next track in an album or playlist 
        // i.e. "Alexa, Next".  This can be expanded to the other AudioPlayer intents, such as repeat, back, etc.

        public override async Task<SpeechletResponse> OnAudioIntentAsync(AudioIntentRequest audioIntentRequest,
    Context context)
        {
            var httpClient = new HttpClient();
            if (audioIntentRequest.Intent.Name == "AMAZON.NextIntent")
            {
                // return await Groove.Music.EnqueueGrooveMusic(context, httpClient, "AMAZON.NextIntent");
            }
            return null;

        }
    }
}