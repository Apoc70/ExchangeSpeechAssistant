﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure4Alexa.Alexa
{
    public class AlexaConstants
    {
        // Inbound requests from Amazon include the Voice Skills AppId, assigned to you when registering your skill
        // Validate the value against what you registered, to ensure that someone else isn't calling your service.  

        // It's bad practice to include the actual AppId in code,
        // but we'll do so here as to make life easy for you.

        public static string AppId = "amzn1.ask.skill.14fc1593-1f8e-4991-9f0f-001ac5f0989e";

        // the value of AppName has no correspondence to what you have registered in Amazon
        // we just store it here because it's useful

        // However, it will appear in the card shown to the user in the Alexa Companion app for iOS or Android
        // You might want to change it

        public static string AppName = "Exchange Speech Assistant";

        // standard error message

        public static string AppErrorMessage = "Sorry, something went wrong.  Please try again.";


    }
}