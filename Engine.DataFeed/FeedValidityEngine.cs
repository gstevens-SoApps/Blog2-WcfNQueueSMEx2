/*
Engine.FeedValidityEngine.cs
  
Copyright 2015 George Stevens

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Contract.DataFeed;
using GS.DataAccess.DataFeed;

namespace GS.Engine.DataFeed
{
    public class FeedValidityEngine : IFeedValidityEngine
    {
        FeedProcessingMsg IFeedValidityEngine.CheckValidity(TestMessage msg, DateTime receivedDateTime)
        {
            Debug.Assert(msg != null);
            Debug.Assert(receivedDateTime != DateTime.MinValue);

            FeedProcessingMsg processingMsg =
                 new FeedProcessingMsg
                 {
                     MessageReceivedDateTime = receivedDateTime,
                     TheMessage = msg
                 };
            processingMsg.IsValid = true;
            if (msg != null)

            {
                if (msg.MsgBody.Contains("Bad Message"))
                {
                    processingMsg.IsValid = false;
                    processingMsg.ErrorMessage = "MessageBody contained 'Bad Message'";
                }

                // TODO 5-17-15 George.  Do other validity checks as well?
                // Other fields set OK.
            }
            else
            {
                processingMsg.IsValid = false;
                processingMsg.ErrorMessage = "MsgBody could not be case to TestMessage.  Was null.";
            }
            return processingMsg;
        }
    }
}
