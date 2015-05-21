﻿/*
GS.DataAccess.DataFeed.IngestedDataDA
  
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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Diagnostics;
using ServiceModelEx;

namespace GS.DataAccess.DataFeed
{
    [GenericResolverBehavior] 
    public class IngestedDataDA : IIngestedDataDA
    {
        private string m_ThisName = "IngestedDataDA";

        void IIngestedDataDA.SaveTestMessage(InProcessFeedMsg msg)
        {
            TestMessageEntity msgEntity = null;
            string moreExInfo = string.Empty;
            try
            {
                msgEntity = MakeTestMessageEntity(msg);
                CloudTable table = 
                    AzureStorageHelpers.ObtainTableCreatIfNotExist(ConstantsNEnums.TestMessageTableName);
                TableOperation insertOperation = TableOperation.Insert(msgEntity);
                table.Execute(insertOperation);
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("409"))
                {
                    moreExInfo = "\n HTTP 409 error code: Likely RowKey already exists on insert.";
                }
                DisplaySaveTestMessageException(ex, moreExInfo);
                throw;
            }
            catch (Exception ex)
            {
                DisplaySaveTestMessageException(ex, null);
                throw;
            }
        }

        private void DisplaySaveTestMessageException(Exception ex, string moreInfo)
        {
            string errMsg = String.Format("{0}.SaveTestMessage(): Caught exception:  ex = {1}",
                m_ThisName, ex);
            Console.WriteLine(errMsg);
            Trace.TraceError("\n**" + errMsg);
            if (!string.IsNullOrEmpty(moreInfo))
            {
                Console.WriteLine(moreInfo);
                Trace.TraceError("\n**" + moreInfo);
            }
        }

        private TestMessageEntity MakeTestMessageEntity(InProcessFeedMsg msg)
        {
            TestMessageEntity msgEntity = null;
            // TODO 5-20-15 George Remove down cast.
            TestMessage testMsg = msg.MessageInProcess as TestMessage;
            if (testMsg != null)
            {
                msgEntity = new TestMessageEntity(testMsg.SourceGroupId,
                                                  testMsg.SourceId,
                                                  testMsg.MessageSendDateTime);
                Debug.Assert(testMsg != null);
                Debug.Assert(msgEntity != null);

                // From SbMessage
                msgEntity.MessageId = msg.MessageInProcess.MessageId;
                msgEntity.MessageSendDateTime = testMsg.MessageSendDateTime;

                // From info on server
                msgEntity.MessageReceivedDateTime = msg.MessageReceivedDateTime;
                // Azure Table Storage does not support TimeSpan.
                TimeSpan etTimeSpan = msg.MessageReceivedDateTime - testMsg.MessageSendDateTime;
                msgEntity.ElapsedTimeString = etTimeSpan.Days + "d:";
                msgEntity.ElapsedTimeString += etTimeSpan.Hours + "h:";
                msgEntity.ElapsedTimeString += etTimeSpan.Minutes + "m:";
                msgEntity.ElapsedTimeString += etTimeSpan.Seconds + "s:";
                msgEntity.ElapsedTimeString += etTimeSpan.Milliseconds + "ms";

                // From TestMessage
                msgEntity.MsgBody = testMsg.MsgBody;
                msgEntity.SourceGroupId = testMsg.SourceGroupId;
                msgEntity.SourceId = testMsg.SourceId;
                msgEntity.SourceMsgSeqNumber = testMsg.SourceMsgSeqNumber;
            }
            else
            {
                msgEntity = new TestMessageEntity();
                msgEntity.ErrorText = "MakeTestMessageEntity(): Bad Message. Could not downcast to TestMessage and ";
                msgEntity.ErrorText += msg.ErrorMessage;
                msgEntity.MessageReceivedDateTime = msg.MessageReceivedDateTime;

                Debug.Assert(false, "Cannot downcast to TestMessage.");
            }
            return msgEntity;
        }
    }
}
