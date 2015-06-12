/*
GS.DataAccess.DataFeedAdmin.FeedAdminDA
  
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

using System.Diagnostics;
using GS.Contract.Admin;
using GS.DataAccess.FeedAdmin;
using GS.iFX.Service;

namespace GS.DataAccess.Common
{
    public class AdminDA : WcfNQueueSMEx2Service, IFeedAdminDA
    {
        private IFeedAdminRepos m_FeedAdminRepository;

        public AdminDA()
        {
            // The purpose for adding the indirection of a repository is to
            // decouple the DataAccessor from the specific type of data base
            // or service api that provides the data.  With the decoupling
            // achieved by programming to the interface it is easy
            // to change service providers without many code changes.
            m_FeedAdminRepository = new FeedAdminRepositoryAzSb();
        }

        #region IFeedAdminDA Members

        long IFeedAdminDA.GetQueueLength(string queueName)
        {
            Debug.Assert(!string.IsNullOrEmpty(queueName));
            return m_FeedAdminRepository.GetQueueLength(queueName);
        }

        DataFeedStatistics IFeedAdminDA.GetFeedStatistics(string queueName)
        {
            Debug.Assert(!string.IsNullOrEmpty(queueName));
            DataFeedStatistics stats = m_FeedAdminRepository.GetFeedStatistics(queueName);
            return stats;
        }
        #endregion
    }
}
