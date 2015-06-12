/*
GS.iFX.Host.DataFeedServiceHost.Program
  
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

using System.ServiceModel;
using GS.Contract.DataFeed;
using GS.DataAccess.Common;
using GS.iFX.TestUI;
using GS.Manager.DataFeed;
using ServiceModelEx.ServiceBus;
using System;
using ConstantsNEnums = GS.iFX.Common.ConstantsNEnums;

namespace GS.iFX.Host.DataFeedServiceHost
{
    public class Program
    {
        // The GenericResolver requires the ServiceHost to have references to assemblies containing
        // participants in complex types that need resolution so that the resolver can build a list 
        // of such types. Just putting an assembly containing participants in the project References
        // is not enough.  You need a single piece of code that access a single item in each assembly 
        // containing participants in complex types.  The code does NOT need a reference to each
        // complex type participant type within an assembly, but rather it needs only to a 
        // reference to any type in the assembly.  That is sufficient to ensure the GenericResolver
        // will work properly in a non-web/non-workerrole situation. 
        //
        // For web and workerrole situations, the assembly name must be prefixed with "App_Code."
        // That is all that needs to be done.  Variables here are not required.
        private static Contract.DataFeed.SbMessage m_SbMessage = null;
        private static DataAccess.Common.InProcessFeedMsg m_InProcessFeedMsg = null;

        private static string m_ThisName = "DataFeedServiceHost";

        static void Main(string[] args)
        {
            // Get rid of compiler warnings by using the GenericResolver related variables.
            m_SbMessage = new TestMessage();
            m_InProcessFeedMsg = new InProcessFeedMsg();

            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Entered. Awaiting your input to start the", m_ThisName);
            Console.WriteLine("  QueuedServicsBusHost for the Service Bus queue '{0}'\n  via WCF NetMessagingBinding.",
                                 ConstantsNEnums.IngestionQueueName);
            ConsoleNTraceHelpers.PauseTillUserPressesEnter();
            QueuedServiceBusHost queuedHost = null;
            try
            {
                Uri sbBaseAddr = new Uri("sb://AzExploreSbNS.servicebus.windows.net/");
                queuedHost = new QueuedServiceBusHost(typeof(DataFeedManager), false, sbBaseAddr);
                queuedHost.Open();
                Console.WriteLine("{0}.Main():  QueuedServiceBusHost opened OK.  Working.....", m_ThisName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex);
            }
            
            Console.WriteLine("\n{0}.Main():  Press ENTER to EXIT.", m_ThisName);
            Console.ReadLine();
            CloseOrAbortHosts(queuedHost);
            Console.WriteLine("\n{0}.Main(): Exiting......", m_ThisName);
        }

        private static void CloseOrAbortHosts(QueuedServiceBusHost queuedHost)
        {
            if (queuedHost != null)
            {
                if (queuedHost.State != CommunicationState.Faulted)
                {
                    queuedHost.Close();
                }
                else
                {
                    queuedHost.Abort();
                }
            }
        }
    }
}