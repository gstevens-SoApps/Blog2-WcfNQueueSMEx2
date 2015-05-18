using GS.Contract.DataFeed;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Contract.DataFeed.Server;

namespace GS.Engine.FeedValidityEngine
{
    public class FeedValidityEngine : IFeedValidityEngine
    {
        FeedProcessingMsg IFeedValidityEngine.CheckValidity(FeedProcessingMsg msg)
        {
            msg.IsValid = true;
            TestMessage testMessage = msg.TheMessage as TestMessage;
            if (testMessage != null)
            {
                if (testMessage.MsgBody.Contains("Bad Message"))
                {
                    msg.IsValid = false;
                    msg.ErrorMessage = "MessageBody contained 'Bad Message'";
                }

                // TODO 5-17-15 George.  Do other validity checks as well?
                // Other fields set OK.
            }
            else
            {
                msg.IsValid = false;
                msg.ErrorMessage = "MsgBody could not be case to TestMessage.  Was null.";
            }
            return msg;
        }
    }
}
