/*
GS.Manager.DataFeed.DataFeedManager
  
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
using GS.DataAccess.DataFeedAdmin;
using GS.Engine.Common;
using GS.iFX.TestUI;
using ServiceModelEx;
using System;
using System.Diagnostics;

namespace GS.Manager.DataFeed
{
    [GenericResolverBehavior]
    public class DataFeedManager : IDataFeeds, IFeedAdmin
    {
        private string m_ThisName = "DataFeedsManager";

        #region IDataFeeds Members

        // Note how the DataFeedManager service composes other services. The Engine and DataAccessor
        // services are hosted and run "in process" to this DataFeedManager service via
        // the ServiceModelEx InProcFactory. Please see "Programming WCF Services", 3rd Edition
        // by Juval Lowy, pp 71-74 for more information.

        void IDataFeeds.IngestTestData(TestMessage msg)
        {
            DateTime msgReceivedTime = DateTime.Now;
            Debug.Assert(msg != null);

            //string displayMsg = string.Format("{0}.IngestTestData(): Entered:", m_ThisName);
            //ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(displayMsg);
            
            // Check validity.
            InProcessFeedMsg checkedMsg = null;
            IValidityEngine validityEngProxy = InProcFactory.CreateInstance<ValidityEngine, IValidityEngine>();
            checkedMsg = validityEngProxy.CheckTestMessageValidity(msg, msgReceivedTime);
            InProcFactory.CloseProxy(validityEngProxy);

            Debug.Assert(checkedMsg != null);
            if (checkedMsg.IsValid)
            {
                // Used only with a test client.   
                DisplayMsgAndQueueLength(msg, checkedMsg);

                // Save data.
                IIngestedDataDA ingestDataDaProxy = InProcFactory.CreateInstance<IngestedDataDA, IIngestedDataDA>();
                ingestDataDaProxy.SaveTestData(checkedMsg);
                InProcFactory.CloseProxy(ingestDataDaProxy);
            }
            else
            {
                Debug.Assert(checkedMsg.IsValid);
                LogInvalidMessageError(checkedMsg);
            }
        }

        private static void DisplayMsgAndQueueLength(TestMessage msg, InProcessFeedMsg checkedMsg)
        {
            // For test client only. 
            IFeedAdminDA feedAdminDaProxy = InProcFactory.CreateInstance<FeedAdminDA, IFeedAdminDA>();
            long queueLength = feedAdminDaProxy.GetQueueLength(iFX.Common.ConstantsNEnums.IngestionQueueName);
            InProcFactory.CloseProxy(feedAdminDaProxy);
            checkedMsg.QueueLength = queueLength;
            string queueLengthMsg = string.Format("{0} count = {1}", 
                                                iFX.Common.ConstantsNEnums.IngestionQueueName, 
                                                queueLength);
            ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(queueLengthMsg);
            ConsoleNTraceHelpers.DisplayTestMessage(msg);
            ConsoleNTraceHelpers.TraceTestMessage(msg);
        }

        private void LogInvalidMessageError(InProcessFeedMsg checkedMsg)
        {
            string errMsg =
                string.Format("{0}.IngestTestData():  Invalid message -- ErrorMessage={1}, Received={2}",
                                m_ThisName, checkedMsg.ErrorMessage, checkedMsg.MessageReceivedDateTime);
            Trace.TraceError(errMsg);
            Console.WriteLine(errMsg);
        }
        #endregion

        #region IFeedAdmin Members

        DataFeedStatistics IFeedAdmin.PresentFeedComponentInfo(string componentName)
        {
            //ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace("\n" + m_ThisName + ".PresentFeedComponentInfo(): Entered:");

            IFeedAdminDA feedAdminDaProxy = InProcFactory.CreateInstance<FeedAdminDA, IFeedAdminDA>();
            DataFeedStatistics stats = feedAdminDaProxy.GetFeedStatistics(componentName);
            InProcFactory.CloseProxy(feedAdminDaProxy);
            return stats;
        }
        #endregion
    }
}
