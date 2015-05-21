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

using GS.Ifx.Common;
using GS.Ifx.UI;
using GS.Manager.DataFeed;
using ServiceModelEx.ServiceBus;
using System;

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
        private static Contract.DataFeed.SbMessage sbMessage = null;
        private static DataAccess.DataFeed.InProcessFeedMsg inProcessFeedMsg = null;

        private static string m_ThisName = "DataFeedServiceHost";

        static void Main(string[] args)
        {
            Console.Title = m_ThisName;
            Console.WriteLine("{0}.Main(): Entered. Awaiting your input to start the", m_ThisName);
            Console.WriteLine("  QueuedServicsBusHost for the Service Bus queue '{0}'\n  via WCF NetMessagingBinding.",
                                ConstantsNEnums.IngestionQueueName);
            ConsoleNTraceHelpers.PauseTillUserPressesEnter();
            QueuedServiceBusHost host = null;
            try
            {
                host = new QueuedServiceBusHost(typeof(DataFeedManager));
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