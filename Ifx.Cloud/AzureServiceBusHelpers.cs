using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace GS.Ifx.Cloud
{
    // George 5-31-15.  The static AzureServiceBusHelpers exist so
    // that many components throughout the system can reuse the boiler
    // plate code involved in doing service bus operations.  Thus, those components
    // do not have to depend upon code hard coded into a respository or
    // duplicated throughout the system.  Note there is no business logic
    // in these helpers.

    // Note, this is "push down", information hiding programming.  Hiding the how and what.  
    // D.L. Parnas would like it.

    public static class AzureServiceBusHelpers
    {
        // TODO George 5-31-15 Get the connection string from an app.config.
        private const string ConnectionString =
                "Endpoint=sb://azexploresbns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=agcbG5SUSKB7AyaT9zkOfQ4Cqj4P5M4TAsxPGOKmUYs=";
        public static long GetQueueLength(string queueName)
        {
            var nsmgr = NamespaceManager.CreateFromConnectionString(ConnectionString);
            long count = nsmgr.GetQueue(queueName).MessageCount;
            return count;
        }

        public static QueueDescription GetQueueDescription(string queueName)
        {
            var nsmgr = NamespaceManager.CreateFromConnectionString(ConnectionString);
            QueueDescription queueDescr = nsmgr.GetQueue(queueName);
            return queueDescr;
        }
    }
}
