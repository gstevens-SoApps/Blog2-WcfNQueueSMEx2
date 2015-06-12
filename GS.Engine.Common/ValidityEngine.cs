/*
GS.Engine.Common.ValidityEngine
  
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

using GS.Contract.DataFeed;
using GS.DataAccess.Common;
using System;
using System.Diagnostics;
using GS.iFX.Service;
using GS.iFX.TestUI;
using ServiceModelEx;

namespace GS.Engine.Common
{
    [GenericResolverBehavior]
    public class ValidityEngine : WcfNQueueSMEx2Service, IFeedValidityEngine, IAdminValidityEngine
    {
        private string m_ThisName = "ValidityEngine";

        #region IFeedValidityEngine Members

        InProcessFeedMsg IFeedValidityEngine.IsTestMessageValid(TestMessage msg, DateTime receivedDateTime)
        {
            Debug.Assert(msg != null);
            Debug.Assert(receivedDateTime != DateTime.MinValue);

            InProcessFeedMsg inProcessMsg = new InProcessFeedMsg
                                                 {
                                                     MessageReceivedDateTime = receivedDateTime,
                                                     MessageInProcess = msg
                                                 };
            inProcessMsg.IsValid = false;
            if (msg != null)
            {
                if (msg.MsgBody.Contains("Bad Message"))
                {
                    inProcessMsg.ErrorMessage = "MessageBody contained 'Bad Message'";
                }
                else if (receivedDateTime == DateTime.MinValue)
                {
                    inProcessMsg.ErrorMessage = "receivedDateTime == DateTime.MinValue";
                }
                // In a real app there would likely be more validity checks.
                else
                {
                    inProcessMsg.IsValid = true;
                }
            }
            else
            {
                inProcessMsg.ErrorMessage = "MsgBody could not be cast to TestMessage.  Was null.";
            }
            return inProcessMsg;
        }
        #endregion

        #region IAdminValidityEngine Members

        void IAdminValidityEngine.IsPresentFeedComponentInfoRequestValid(string componentName)
        {
            // In a real app one would likely have the FeedAdminDA check to see if the componentName
            // was an active Feed Component in the system and throw an exception if not.  
            // That's too much work for a demo.  This suffices to show the concept and test it.
            if (string.IsNullOrEmpty(componentName))
            {
                ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(m_ThisName + ".IAdminValidityEngine.IsPresentFeedComponentInfoRequestValid():  arg was null or empty.");
                throw new ArgumentNullException("componentName", "IAdminValidityEngine -- String was null or empty.");
            }
        }
        #endregion
    }
}
