/*
GS.DataAccess.DataFeedAdmin.FeedAdminRepositoryAzSb
  
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
using Microsoft.ServiceBus.Messaging;
using System;
using GS.Ifx.UI;

namespace GS.DataAccess.DataFeedAdmin
{
    // George 5-31-15.  Please see the comments in FeedAdminDA for more about the purpose
    // of this class.

    // George 5-31-15.  The static AzureServiceBusHelpers exist so
    // that other components throughout the system can reuse the boiler
    // plate code involved in doing these service bus operations.  Thus, they
    // do not have to depend upon code hard coded into this respository or
    // duplicated throughout the system.  Note there is no business logic
    // in these helpers.

    public class FeedAdminRepositoryAzSb : IFeedAdminRepository
    {
        private string m_ThisName = "FeedAdminRepositoryAzSb";

        #region IFeedAdminRepository Members

        long IFeedAdminRepository.GetQueueLength(string queueName)
        {
            try
            {
                QueueDescription queueDescr = Ifx.Cloud.AzureServiceBusHelpers.GetQueueDescription(queueName);
                long count = queueDescr.MessageCount;
                return count;
            }
            catch (Exception ex)
            {
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".GetQueueLength()", ex);
                throw;
            }
        }

        DataFeedStatistics IFeedAdminRepository.GetFeedStatistics(string queueName)
        {
            try
            {
                DataFeedStatistics stats = new DataFeedStatistics();
                stats.FeedComponentName = queueName;
                stats.StatsCollectionDateTime = DateTime.Now;
                QueueDescription queueDescr = Ifx.Cloud.AzureServiceBusHelpers.GetQueueDescription(queueName);
                stats.QueueLength = queueDescr.MessageCount;
                stats.DeadLetterQueueLength = queueDescr.MessageCountDetails.DeadLetterMessageCount;
                return stats;
            }
            catch (Exception ex)
            {
                ConsoleNTraceHelpers.DisplayExToConsoleNTrace(m_ThisName + ".GetFeedStatistics()", ex);
                throw;
            }
        }
        #endregion
    }
}
