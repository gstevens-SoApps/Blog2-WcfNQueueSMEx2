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

        void IDataFeeds.IngestTestData(TestMessage msg)
        {
            DateTime msgReceivedTime = DateTime.Now;
            Debug.Assert(msg != null);

            ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace("\n" + m_ThisName + ".IngestTestData(): Entered:");

            // For stress test comparison with elapsed time tests.
            //return;

            // TODO George 6-2-15 -- Put in the blog on this.
            // Below note the composition of microservices!  Specifially, 
            // the Engine and DataAccessor microservices are hosted and run within
            // this DataFeedManager microservice via the ServiceModelEx
            // InProcFactory.  The DataFeedManager instance thus contains these 2
            // other microservices, but only for as long as they are open!
            // Please see "Programming WCF Services" 3rd Edition by Juval Lowy, 
            // pp 71-74 for more information.
            //
            // One good reason to run the Engine and DataAccessor microservices in 
            // process to the DataFeedManager as WCF services (i.e. hosting WCF
            // services within this DataFeedManager) is that it  allows any WCF
            // message headers (comming all the way from the sender of the
            // message) to be available to all the composed microservices. This 
            // technique is very effective in separating the concerns of system info (in the
            // message headers) from business domain info (in the message), and can
            // greatly reduce code clutter and increase programmer productivity and
            // enjoyment.  Sometimes these WCF message headers are called 
            // "out-of-band paramaters".  They can be very useful and with a little
            // rather simple infrastructure, easy to use.
            //
            // However, using such message headers is far beyond the scope
            // of this simple example.  The goal here is to introduce you to
            // one good reason to run WCF services "in process" with another WCF service.

            InProcessFeedMsg checkedMsg = null;
            IValidityEngine validityEngProxy = InProcFactory.CreateInstance<ValidityEngine, IValidityEngine>();
            checkedMsg = validityEngProxy.CheckValidity(msg, msgReceivedTime);
            InProcFactory.CloseProxy(validityEngProxy);

            Debug.Assert(checkedMsg != null);
            if (checkedMsg.IsValid)
            {
                IFeedAdminDA feedAdminDaProxy = InProcFactory.CreateInstance<FeedAdminDA, IFeedAdminDA>();
                long queueLength = feedAdminDaProxy.GetQueueLength(iFX.Common.ConstantsNEnums.IngestionQueueName);
                InProcFactory.CloseProxy(feedAdminDaProxy);
                checkedMsg.QueueLength = queueLength;

                DisplayNTraceQueueLength(queueLength, iFX.Common.ConstantsNEnums.IngestionQueueName);
                ConsoleNTraceHelpers.DisplayTestMessage(msg);
                ConsoleNTraceHelpers.TraceTestMessage(msg);
                
                IIngestedDataDA ingestDataDaProxy = InProcFactory.CreateInstance<IngestedDataDA, IIngestedDataDA>();
                ingestDataDaProxy.SaveTestData(checkedMsg);
                InProcFactory.CloseProxy(ingestDataDaProxy);
            }
            else
            {
                Debug.Assert(checkedMsg.IsValid);
                LogInvalidMessageError(checkedMsg);
            }
            // For slowing down the rate of dequeueing to allow time to see queue length via tcp end pt.
            //Thread.Sleep(500);
        }

        private static void DisplayNTraceQueueLength(long queueLength, string queueName)
        {
            string queueLengthMsg = string.Format("{0} count = {1}", queueName, queueLength);
            Console.WriteLine(queueLengthMsg);
            Trace.TraceInformation("**" + queueLengthMsg);
        }

        private void LogInvalidMessageError(InProcessFeedMsg checkedMsg)
        {
            string errMsg =
                string.Format("\n{0}.IngestTestData():  Invalid message -- ErrorMessage={1}, Received={2}",
                                m_ThisName, checkedMsg.ErrorMessage, checkedMsg.MessageReceivedDateTime);
            Trace.TraceError(errMsg);
            Console.WriteLine(errMsg);
        }
        #endregion

        #region IFeedAdmin Members

        DataFeedStatistics IFeedAdmin.PresentFeedComponentInfo(string componentName)
        {
            ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace("\n" + m_ThisName + ".PresentFeedComponentInfo(): Entered:");

            IFeedAdminDA feedAdminDaProxy = InProcFactory.CreateInstance<FeedAdminDA, IFeedAdminDA>();
            DataFeedStatistics stats = feedAdminDaProxy.GetFeedStatistics(componentName);
            InProcFactory.CloseProxy(feedAdminDaProxy);
            return stats;
        }
        #endregion
    }
}
