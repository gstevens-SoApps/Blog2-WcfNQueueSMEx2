/*
Manager.DataFeed.DataFeedManager
  
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
using GS.DataAccess.DataFeed;
using GS.Engine.DataFeed;
using GS.Ifx.UI;
using ServiceModelEx;
using System;
using System.Diagnostics;

namespace GS.Manager.DataFeed
{
    [GenericResolverBehavior] 
    public class DataFeedManager : IDataFeeds
    {
        private string m_ThisName = "DataFeedsManager";

        void IDataFeeds.IngestTestData(TestMessage msg)
        {
            DateTime msgReceivedTime = DateTime.Now;
            Debug.Assert(msg != null);

            string greeting = String.Format("\n{0}.IngestTestData(): Entered.", m_ThisName);
            Console.WriteLine(greeting);
            Trace.TraceInformation("**" + greeting);
            ConsoleNTraceHelpers.DisplayTestMessage(msg);
            ConsoleNTraceHelpers.TraceTestMessage(msg);

            // The business logic.
            
            // Validate message.
            IFeedValidityEngine validityEngine = new FeedValidityEngine();
            FeedProcessingMsg checkedMsg = validityEngine.CheckValidity(msg, msgReceivedTime);

            // Save message.
            IIngestedDataDA ingestedDataDA = new IngestedDataDA();
            if (checkedMsg.IsValid)
            {
                ingestedDataDA.SaveTestMessage(checkedMsg);
            }
            else
            {
                Debug.Assert(checkedMsg.IsValid);

                LogInvalidMessageError(checkedMsg);
                string errMsg = "Saving invalid message to BadMessage table.";
                Trace.TraceInformation(errMsg);
                Console.WriteLine(errMsg);

                ingestedDataDA.SaveBadMessage(checkedMsg);
            }
        }

        private void LogInvalidMessageError(FeedProcessingMsg checkedMsg)
        {
            string errMsg =
                string.Format("\n{0}.IngestTestData():  Invalid message -- ErrorMessage={1}, Received={2}",
                                m_ThisName,checkedMsg.ErrorMessage, checkedMsg.MessageReceivedDateTime);
            Trace.TraceError(errMsg);
            Console.WriteLine(errMsg);
        }

        // TODO 5-17-15 George.  The NetMessagingBinding requires OneWay=true. 
        // Move this operation into another service contract
        // that uses a TCP binding, not NetMessagingBinding.
        // TODO 5-17-15 George.  Implement this method.
        //DataFeedStatistics IDataFeeds.CollectDataFeedStatistics()
        //{
        //    string greeting = String.Format("\n{0}.CollectFeedStatistics(): Entered.", m_ThisName);
        //    Console.WriteLine(greeting);
        //    Trace.TraceInformation("**" + greeting);
        //    Console.WriteLine("Not yet implemented.");
        //    Trace.TraceInformation("Not yet implemented.");
        //    DataFeedStatistics stats = new DataFeedStatistics();
        //    return stats;
        //}
    }
}
