/*
GS.Ifx.Host.DataFeedServiceHost
  
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
using GS.Ifx.UI;
using GS.Manager.DataFeed;
using ServiceModelEx;
using ServiceModelEx.ServiceBus;
using System;
using ConstantsNEnums = GS.Ifx.Common.ConstantsNEnums;

namespace GS.Ifx.Host.DataFeedServiceHost
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
        // For web and workerrole situations, the assemblies must be prefixed with "App_Code."
        // That is all that needs to be done?
        private static Contract.DataFeed.SbMessage m_SbMessage = null;
        private static DataAccess.Common.InProcessFeedMsg m_InProcessFeedMsg = null;

        private static string m_ThisName = "DataFeedServiceHost";

        static void Main(string[] args)
        {
            // Get rid of compiler warnings vy using the GenericResolver related variables.
            m_SbMessage = new TestMessage();
            m_InProcessFeedMsg = new InProcessFeedMsg();

            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Entered. Awaiting your input to start the", m_ThisName);
            Console.WriteLine("  QueuedServicsBusHost for the Service Bus queue '{0}'\n  via WCF NetMessagingBinding.",
                                 ConstantsNEnums.IngestionQueueName);
            ConsoleNTraceHelpers.PauseTillUserPressesEnter();
            QueuedServiceBusHost queuedHost = null;
            ServiceHost<DataFeedManager> feedAdminHost = null;
            try
            {
                Uri sbBaseAddr = new Uri("sb://AzExploreSbNS.servicebus.windows.net/");
                queuedHost = new QueuedServiceBusHost(typeof(DataFeedManager), false, sbBaseAddr);
                queuedHost.Open();
                Console.WriteLine("{0}.Main():  QueuedServiceBusHost opened OK.  Working.....", m_ThisName);

                Uri tcpBaseAddr = new Uri("net.tcp://localhost:8000/");
                feedAdminHost = new ServiceHost<DataFeedManager>(tcpBaseAddr);
                // When a non-queued endpoint is defined in the App.config for a service that
                // is hosted in the QueuedServiceBusHost, this host will try to open that endpoint too.
                // This will cause a timeout exception when it tries to verify the queue (which does
                // not exist for a non-queued endpoint like TCP).  Therefore, one cannot
                // use the app.config file to define non-queued endpoints in this situation.
                // A workaround is the tcp endpoint for IFeedAdmin is defined in code instead, below.
                // The client can, however, define the tcp endpoint in their app.config file.

                // Transport security is the default, but a required arg to set Reliable = true.
                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.Transport, true);
                tcpBinding.TransactionFlow = true;
                feedAdminHost.AddServiceEndpoint(typeof(IFeedAdmin), tcpBinding, "DataFeedManager");
                feedAdminHost.Open();
                Console.WriteLine("{0}.Main():  FeedAdmin Host opened OK.", m_ThisName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex.ToString());
            }
            
            Console.WriteLine("\n{0}.Main():  Press ENTER to EXIT.", m_ThisName);
            Console.ReadLine();
            CloseOrAbortHosts(queuedHost, feedAdminHost);
            Console.WriteLine("\n{0}.Main(): Exiting......", m_ThisName);
        }

        private static void CloseOrAbortHosts(QueuedServiceBusHost queuedHost, 
                                              ServiceHost<DataFeedManager> feedAdminHost)
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
            if (feedAdminHost != null)
            {
                if (feedAdminHost.State != CommunicationState.Faulted)
                {
                    feedAdminHost.Close();
                }
                else
                {
                    feedAdminHost.Abort();
                }
            }
        }
    }
}