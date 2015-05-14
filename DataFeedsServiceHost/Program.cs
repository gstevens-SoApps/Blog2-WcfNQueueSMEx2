/*
DataFeedsServiceHost.Program
  
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
using ServiceModelEx.ServiceBus;
using Shared;
using System;
using MiscHelpers;

namespace DataFeedsServiceHost
{
    public class Program
    {
        private static string m_ThisName = "DataFeedsServiceHost";

        static void Main(string[] args)
        {
            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Entered. Awaiting your input to start the", m_ThisName);
            Console.WriteLine("  QueuedServicsBusHost for the Service Bus queue '{0}'\n  via WCF NetMessagingBinding.",
                                ConstsNEnums.IngestionQueueName);
            ConsoleNTraceHelpers.PauseTillUserPressesEnter();
            QueuedServiceBusHost host = null;
            try
            {
                host = new QueuedServiceBusHost(typeof(DataFeedsManager.DataFeedsManager));
                host.Open();
                Console.WriteLine("{0}.Main():  QueuedServiceBusHost opened OK.  Working.....", m_ThisName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex.ToString());
            }
            
            Console.WriteLine("\n{0}.Main():  Press ENTER to EXIT.", m_ThisName);
            Console.ReadLine();
            if (host != null)
            {
                host.Close();
            }
            Console.WriteLine("\n{0}.Main(): Exiting......", m_ThisName);
        }
    }
}