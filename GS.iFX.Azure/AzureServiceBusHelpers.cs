/*
GS.iFX.Azure.AzureServiceBusHelpers
 * 
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

using System.Configuration;
using System.Diagnostics;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace GS.iFX.Azure
{
    // George 5-31-15.  The static AzureServiceBusHelpers exist so
    // that many components throughout the system can reuse the boiler
    // plate code involved in doing service bus operations.  Thus, those components
    // do not have to depend upon code hard coded into a respository or
    // duplicated throughout the system.  Note there is no business logic
    // in these helpers.

    // Note, this is "push down", information hiding programming.  Hiding the how and where.  
    // David Parnas would like it.

    public static class AzureServiceBusHelpers
    {
        private static string m_ConnectionString;

        static AzureServiceBusHelpers()
        {
            m_ConnectionString = GetSbConnectionString();
            Debug.Assert(!string.IsNullOrEmpty(m_ConnectionString));
        }
       
        public static long GetQueueLength(string queueName)
        {
            var nsmgr = NamespaceManager.CreateFromConnectionString(m_ConnectionString);
            long count = nsmgr.GetQueue(queueName).MessageCount;
            return count;
        }

        public static QueueDescription GetQueueDescription(string queueName)
        {
            
            var nsmgr = NamespaceManager.CreateFromConnectionString(m_ConnectionString);
            QueueDescription queueDescr = nsmgr.GetQueue(queueName);
            return queueDescr;
        }

        private static string GetSbConnectionString()
        {
            // The app.config file resides in the service host of the service calling this.
            string connString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            return connString;
        }
    }
}
