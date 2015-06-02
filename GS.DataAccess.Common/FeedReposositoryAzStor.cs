/*
GS.DataAccess.DataFeed.FeedReposositoryAzStor 
  
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
using GS.Ifx.Cloud;
using GS.Ifx.UI;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Diagnostics;

namespace GS.DataAccess.Common
{
    public class FeedReposositoryAzStor : IFeedReposository
    {
        private string m_ThisName = "FeedReposositoryAzStor";

        void IFeedReposository.SaveTestData(InProcessFeedMsg msg)
        {
            TestDataEntity msgEntity = null;
            string moreExInfo = string.Empty;
            try
            {
                // TODO 5-22-15 bubble isPerformanceTest argument through somehow.  In msg or out of band.
                //msgEntity = MakeTestMessageEntity(msg, true);
                msgEntity = MakeTestDataEntity(msg, false);

                // TODO 5-23-15 Failsafe requires using Transient Fault application block and
                // maybe circuit breaker pattern on all requests for cloud services.  Wrap below
                // code in that stuff as an example.
                AzureStorageHelpers.AzTableStorageInsert(ConstantsNEnums.TestMessageTableName, msgEntity);
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("409"))
                {
                    moreExInfo = "\n HTTP 409 error code: Likely RowKey already exists on insert.";
                }
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".SaveTestData()", ex);
                ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(moreExInfo);
                throw;
            }
            catch (Exception ex)
            {
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".SaveTestData()", ex);
                throw;
            }
        }

        private TestDataEntity MakeTestDataEntity(InProcessFeedMsg msg, bool isPerformanceTest)
        {
            TestDataEntity msgEntity = null;

            TestMessage testMsg = msg.MessageInProcess as TestMessage;
            if (testMsg != null)
            {
                // The intent below is that the received date time will be unique due to
                // the sequential nature of the queue.  However for basic testing it is
                // sometimes nice to see ordering of the table in terms of send date time.
                DateTime rowKey;
                if (isPerformanceTest)
                {
                    rowKey = msg.MessageReceivedDateTime;
                }
                else
                {
                    rowKey = testMsg.MessageSendDateTime;
                }
                msgEntity = new TestDataEntity(testMsg.SourceGroupId,
                    testMsg.SourceId,
                    rowKey);
                Debug.Assert(testMsg != null);
                Debug.Assert(msgEntity != null);

                // From SbMessage
                msgEntity.MessageId = msg.MessageInProcess.MessageId;
                msgEntity.MessageSendDateTime = testMsg.MessageSendDateTime;

                // From info on server
                msgEntity.MessageReceivedDateTime = msg.MessageReceivedDateTime;
                // Azure Table Storage does not support TimeSpan.
                TimeSpan etTimeSpan = msg.MessageReceivedDateTime - testMsg.MessageSendDateTime;
                msgEntity.EtString = etTimeSpan.Days + "d:";
                msgEntity.EtString += etTimeSpan.Hours + "h:";
                msgEntity.EtString += etTimeSpan.Minutes + "m:";
                msgEntity.EtString += etTimeSpan.Seconds + "s:";
                msgEntity.EtString += etTimeSpan.Milliseconds + "ms";

                msgEntity.EtSeconds = etTimeSpan.TotalSeconds;

                msgEntity.QueueLength = msg.QueueLength;

                // From TestMessage
                msgEntity.MsgBody = testMsg.MsgBody;
                msgEntity.SourceGroupId = testMsg.SourceGroupId;
                msgEntity.SourceId = testMsg.SourceId;
                msgEntity.SourceMsgSeqNumber = testMsg.SourceMsgSeqNumber;
            }
            else
            {
                msgEntity = new TestDataEntity();
                msgEntity.ErrorText = "MakeTestDataEntity(): Bad Message. Could not downcast to TestMessage and ";
                msgEntity.ErrorText += msg.ErrorMessage;
                msgEntity.MessageReceivedDateTime = msg.MessageReceivedDateTime;

                Debug.Assert(false, "Cannot downcast to TestMessage.");
            }
            return msgEntity;
        }
    }
}
