/*
GS.DataAccess.Common.IngestedDataDA
  
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

using GS.iFX.Service;
using ServiceModelEx;
using System.Diagnostics;

namespace GS.DataAccess.Common
{
    [GenericResolverBehavior]
    public class IngestedDataDA : WcfNQueueSMEx2Service, IIngestedDataDA
    {
        private IIngestedRepos m_FeedRepository;
        private string m_ThisName = "IngestedDataDA";

        public IngestedDataDA()
        {
            // The purpose for adding the indirection of a repository is to
            // decouple the DataAccessor from the specific type of data base
            // or service api that provides the data.  With the decoupling
            // achieved by programming to the interface it is easy
            // to change service providers without many code changes.

            // A more flexible design to use in real life is to use an
            // abstract factory to create the storage specific repository based upon
            // configuration data residing in a database or config file.  The
            // abstract factory can then be either dependency injected or coded in the ctor.
            // In either case use an out-of-band parameter (in the message header)
            // to determine which storage specific concrete instance of the repository to create.
            // Here's an article about abstract factories:
            // https://dotnetsilverlightprism.wordpress.com/2013/05/18/solid-abstract-factory-strategy-pattern-dependency-injection/
            
            m_FeedRepository = new IngestedReposAzStor();
        }

        #region IIngestedDataDA Members

        void IIngestedDataDA.SaveTestData(InProcessFeedMsg msg)
        {
            Debug.Assert(msg != null);
            string displayMsg = string.Format("{0}.SaveTestData(): Entered:\n", m_ThisName);
            //ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(displayMsg);
            
            m_FeedRepository.SaveTestData(msg);
        }

        string IIngestedDataDA.GetDummyDataForAnalysis()
        {
            return "This is the dummy ingested data for analysis.";
        }
        #endregion
    }
}
