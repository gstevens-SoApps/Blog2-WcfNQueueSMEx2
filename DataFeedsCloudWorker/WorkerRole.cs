/*
DataFeedsCloudWorker.WorkerRole
  
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
using Microsoft.WindowsAzure.ServiceRuntime;
using ServiceModelEx.ServiceBus;
using System;
using System.Diagnostics;
using System.Threading;

namespace DataFeedsCloudWorker
{
    public class WorkerRoleWcf : RoleEntryPoint
    {
        private static string m_ThisName = "WorkerRoleWcf";
        private static QueuedServiceBusHost m_QueuedServiceBusHost;

        public override void Run()
        {
            Trace.TraceInformation("\n**" + m_ThisName + ".Run():  Entered.");
            Trace.TraceInformation("\n**" + m_ThisName + ".Run(): Starting QueuedServiceBusHost...");
            
            // The service host is opened in this Run() method so as to have the queue 
            // immediately emptied upon it successfully starting.  The queue will not
            // be emptied upon the service host starting if launched from OnStart().
            bool serviceHostOk = OpenServiceHost();
            if (serviceHostOk)
            {
                try
                {
                    Trace.TraceInformation("\n**" + m_ThisName + ".Run():  Starting forever loop...");
                    while (true)
                    {
                        Trace.TraceInformation("\n**" + m_ThisName + ".Run():  Working....");
                        Thread.Sleep(30000);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("\n**" + m_ThisName + ".Run():  Exception in forever loop!\n" + ex);
                    m_QueuedServiceBusHost.Abort();
                    throw;
                }
            }
            Trace.TraceInformation("\n**" + m_ThisName + ".Run(): Exiting.  serviceHostOk=" + serviceHostOk);
        }

        private bool OpenServiceHost()
        {
            bool serviceHostOk = false;
            try
            {
                m_QueuedServiceBusHost = new QueuedServiceBusHost(typeof(DataFeedsManager.DataFeedsManager));
                m_QueuedServiceBusHost.Open();
                serviceHostOk = true;
                Trace.TraceInformation("\n**" + m_ThisName + ".OpenServiceHost():  QueuedServiceBusHost opened Ok.");
            }
            catch (Exception ex)
            {
                Trace.TraceError("\n**" + m_ThisName + ".OpenServiceHost():  host.Open() Threw exception!\n" + ex);
                m_QueuedServiceBusHost.Abort();
            }
            return serviceHostOk;
        }

        public override bool OnStart()
        {
            Trace.TraceInformation("\n**WorkerRole.OnStart(): Entered.");
            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("\n**WorkerRole.OnStop(): Entered.");
            m_QueuedServiceBusHost.Close();
            base.OnStop();
        }
    }
}
