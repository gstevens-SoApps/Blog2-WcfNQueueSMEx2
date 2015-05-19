/*
DataAccess.DataFeed.IngestedDataDA
  
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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Configuration;
using System.Diagnostics;

namespace GS.DataAccess.DataFeed
{
    public class IngestedDataDA : IIngestedDataDA
    {
        void IIngestedDataDA.SaveTestMessage(FeedProcessingMsg msg)
        {
            TestMessageEntity msgEntity = null;
            try
            {
                msgEntity = MakeTestMessageEntity(msg);
                CloudTable table = ObtainTableCreatIfNotExist("TestMessages");
                TableOperation insertOperation = TableOperation.Insert(msgEntity);
                table.Execute(insertOperation);
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("409"))
                {
                    Console.WriteLine("Caught exception:\n   Likely RowKey already exists on insert.\n  ex = {0}", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception:  ex = {0}", ex);
                throw;
            }
        }

        private TestMessageEntity MakeTestMessageEntity(FeedProcessingMsg msg)
        {
            TestMessageEntity msgEntity = null;

            TestMessage testMsg = msg.TheMessage as TestMessage;
            if (testMsg != null)
            {
                msgEntity = new TestMessageEntity(testMsg.SourceGroupId,
                                                  testMsg.SourceId,
                                                  testMsg.MessageSendDateTime);
                Debug.Assert(testMsg != null);
                Debug.Assert(msgEntity != null);

                // From SbMessage
                msgEntity.MessageId = msg.TheMessage.MessageId;
                msgEntity.MessageSendDateTime = testMsg.MessageSendDateTime;

                // From info on server
                msgEntity.MessageReceivedDateTime = msg.MessageReceivedDateTime;
                // Azure Table Storage does not support TimeSpan.
                TimeSpan etTimeSpan = msg.MessageReceivedDateTime - testMsg.MessageSendDateTime;
                msgEntity.ElapsedTimeString = etTimeSpan.ToString();

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

        // TODO Put this in Infrastructure somewhere.
        private static CloudTable ObtainTableCreatIfNotExist(string tableName)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        // TODO Put this in Infrastructure somewhere.  And use a constant.
        private static CloudStorageAccount GetCloudStorageAccount()
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            return storageAccount;
        }

        // TODO Have a common worker that is called by both saves, passing the table name.
        void IIngestedDataDA.SaveBadMessage(FeedProcessingMsg msg)
        {
            TestMessageEntity msgEntity = null;
            try
            {
                msgEntity = MakeTestMessageEntity(msg);
                CloudTable table = ObtainTableCreatIfNotExist("TestMessages");
                TableOperation insertOperation = TableOperation.Insert(msgEntity);
                table.Execute(insertOperation);
            }
            catch (StorageException ex)
            {
                if (ex.Message.Contains("409"))
                {
                    Console.WriteLine("Caught exception:\n   Likely RowKey already exists on insert.\n  ex = {0}", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception:  ex = {0}", ex);
                throw;
            }
        }
    }
}
