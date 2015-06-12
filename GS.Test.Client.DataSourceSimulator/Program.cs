/*
GS.Test.Client.DataSourceSimulator.Program
  
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
using GS.iFX.Common;
using GS.iFX.TestUI;
using GS.Proxy.DataFeed;
using System;

namespace GS.Test.Client.DataSourceSimulator
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
            Console.WriteLine("{0}.Main(): Press <ENTER> to ENQUEUE one", m_ThisName);
            Console.WriteLine("  test message to SB queue '{0}' via WCF NetMessagingBinding",
                               ConstantsNEnums.IngestionQueueName);
            InitSourceIds(args);

            while (true)
            {
                string userInput = PauseTillUserPressesEnter_StatisticsOnS_ExitOnX().ToLower();
                if (userInput == "x")
                {
                    break;
                }
                else
                {
                    try
                    {
                        // Assume the queue exists, having been previously provisioned in Azure Portal.
                        Console.WriteLine("{0}.Main(): Enqueueing test message..", m_ThisName);

                        TestMessage msg = MakeTestMessage(m_SourceGroupId, m_SourceId, ++m_MsgSeqNumber);
                        IngestTestData(msg, ConstantsNEnums.IngestionQueueEndpointName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}.Main(): IngestTestData() proxy threw exception\n {1}.", m_ThisName, ex);
                    }
                }
            }
        }

        private static void InitSourceIds(string[] args)
        {
            // TODO 5-9-15 parse args for the source ids
            m_SourceId = 9;
            m_SourceGroupId = 99;
        }

        public static TestMessage MakeTestMessage(int sourceGroupId, int sourceId, int sourceSeqNo)
        {
            TestMessage msg = new TestMessage
            {
                MessageId = Guid.NewGuid(),
                SourceMsgSeqNumber = sourceSeqNo,
                SourceId = sourceId,
                SourceGroupId = sourceGroupId,
                MsgBody = "Testing, testing...."
            };
            return msg;
        }

        private static void IngestTestData(TestMessage msg, string queueName)
        {
            // Allow calculation of elapsed time: From proxy instantiation 
            // till the message is received by the instantiated service instance.
            msg.MessageSendDateTime = DateTime.Now;

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
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".IngestTestData(): ", ex);
                proxy.Abort();
            }
        }

        private static string PauseTillUserPressesEnter_StatisticsOnS_ExitOnX()
        {
            Console.WriteLine("Press <ENTER> to ENQUEUE an item, OR enter 'x'<ENTER> to EXIT....");
            string userInput = Console.ReadLine();
            return userInput;
        }
    }
}
