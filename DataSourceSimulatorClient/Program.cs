/*
DataSourceSimulatorClient.Program
  
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
using Shared;
using MiscHelpers;

namespace DataSourceSimulatorClient
{
    public class Program
    {
        private static string m_ThisName = "DataSourceSimulatorClient";
        private static int m_MsgSeqNumber;
        private static int m_SourceId;
        private static int m_SourceGroupId;


        public static void Main(string[] args)
        {
            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Press <ENTER> to enqueue one", m_ThisName);
            Console.WriteLine("  test message to SB queue '{0}' via WCF NetMessagingBinding",
                               ConstsNEnums.IngestionQueueName);
            InitSourceIds(args);

            while (true)
            {
                string userInput = ConsoleNTraceHelpers.PauseTillUserPressesEnterExitOnX();
                if (userInput.ToLower() == "x")
                {
                    break;
                }
                
                try
                {
                    // Assume the queue exists, having been previously provisioned in Azure Portal.
                    Console.WriteLine("{0}.Main(): Enqueueing test message..", m_ThisName);

                    TestMessage msg = MakeTestMessage(++m_MsgSeqNumber, m_SourceId, m_SourceGroupId);
                    IngestTestData(msg, ConstsNEnums.IngestionQueueEndpointName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}.Main(): IngestTestData() proxy threw exception\n {1}.", m_ThisName, ex);
                }
            }
        }

        private static void InitSourceIds(string[] args)
        {
            // TODO 5-9-15 parse args for the source ids
            m_SourceId = 9;
            m_SourceGroupId = 99;
        }

        private static TestMessage MakeTestMessage(int sourceSeqNo, int sourceId, int sourceGroupId)
        {
            TestMessage msg = new TestMessage
            {
                MessageId = Guid.NewGuid(),
                MessageDateTime = DateTime.Now,
                SourceMsgSeqNumber = sourceSeqNo,
                SourceId = sourceId,
                SourceGroupId = sourceGroupId,
                MsgBody = "Testing, testing...."
            };
            return msg;
        }

        private static void IngestTestData(TestMessage msg, string queueName)
        {
            // "Programming WCF Services" 3rd edition by Juval Lowy pp 259-260 recommends the
            // following form when needing to catch exceptions near the SendQueuedTestMessage(). 
            DataFeedsClient proxy = new DataFeedsClient(queueName);
            try
            {
                proxy.IngestTestData(msg);
                proxy.Close();
                Console.WriteLine("{0}.IngestTestData(): Test message enqueued Ok.", m_ThisName);
                ConsoleNTraceHelpers.DisplayTestMessage(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}.IngestTestData(): Proxy threw exception\n {1}.", m_ThisName, ex);
                proxy.Abort();
            }
        }
    }
}
