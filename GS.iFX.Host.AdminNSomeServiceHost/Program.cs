
/*
 GS.iFX.Host.AdminNSomeServiceHost.Program
  
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

using GS.Contract.Admin;
using GS.Contract.Something;
using GS.iFX.TestUI;
using GS.Manager.Admin;
using GS.Manager.Something;
using ServiceModelEx;
using System;
using System.ServiceModel;

namespace GS.iFX.Host.AdminNSomeServiceHost
{
    class Program
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
        private static DataFeedStatistics m_FeedStats = null;
        private static IngestedDataAnalysisResult m_DataAnalysisResult = null;

        private static string m_ThisName = "AdminNSomeServiceHost";
        
        static void Main(string[] args)
        {
            // Get rid of compiler warnings by using the GenericResolver related variables.
            m_FeedStats = new DataFeedStatistics();
            m_DataAnalysisResult = new IngestedDataAnalysisResult();
            
            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Entered. Awaiting your input to start the", m_ThisName);
            Console.WriteLine("Service Host for the AdminManager and SomeManager");
            ConsoleNTraceHelpers.PauseTillUserPressesEnter();

            ServiceHost<AdminManager> adminHost = null;
            ServiceHost<SomeManager> someManagerHost = null;
            try
            {
                adminHost = new ServiceHost<AdminManager>();
                adminHost.Open();
                Console.WriteLine("{0}.Main():  Admin ServiceHost opened OK.", m_ThisName);

                someManagerHost = new ServiceHost<SomeManager>();
                someManagerHost.Open();
                Console.WriteLine("{0}.Main():  SomeManager ServiceHost opened OK.", m_ThisName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex);
            }
            Console.WriteLine("\n{0}.Main():  Press ENTER to EXIT.", m_ThisName);
            Console.ReadLine();
            CloseOrAbortHosts(adminHost, someManagerHost);
            Console.WriteLine("\n{0}.Main(): Exiting......", m_ThisName);
        }

        private static void CloseOrAbortHosts(ServiceHost<AdminManager> adminHost, ServiceHost<SomeManager> someManagerHost)
        {
            if (adminHost != null)
            {
                if (adminHost.State != CommunicationState.Faulted)
                {
                    adminHost.Close();
                }
                else
                {
                    adminHost.Abort();
                }
            }
            if (someManagerHost != null)
            {
                if (someManagerHost.State != CommunicationState.Faulted)
                {
                    someManagerHost.Close();
                }
                else
                {
                    someManagerHost.Abort();
                }
            }
        }

        //Uri tcpBaseAddr = new Uri("net.tcp://localhost:8000/");
        //feedAdminHost = new ServiceHost<DataFeedManager>(tcpBaseAddr);
        //// When a non-queued endpoint is defined in the App.config for a service that
        //// is hosted in the QueuedServiceBusHost, this host will try to open that endpoint too.
        //// This will cause a timeout exception when it tries to verify the queue (which does
        //// not exist for a non-queued endpoint like TCP).  Therefore, one cannot
        //// use the app.config file to define non-queued endpoints in this situation.
        //// A workaround is the tcp endpoint for IFeedAdmin is defined in code instead, below.
        //// The client can, however, define the tcp endpoint in their app.config file.

        //// Transport security is the default, but a required arg to set Reliable = true.
        //NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.Transport, true);
        //tcpBinding.TransactionFlow = true;
        //feedAdminHost.AddServiceEndpoint(typeof(IFeedAdmin), tcpBinding, "DataFeedManager");
        //feedAdminHost.Open();
        //Console.WriteLine("{0}.Main():  FeedAdmin Host opened OK.", m_ThisName);
    }
}
