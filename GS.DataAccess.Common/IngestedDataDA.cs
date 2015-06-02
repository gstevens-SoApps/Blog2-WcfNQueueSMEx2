/*
GS.DataAccess.DataFeed.IngestedDataDA
  
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

using ServiceModelEx;
using System;
using System.Diagnostics;
using GS.Ifx.UI;

namespace GS.DataAccess.Common
{
    [GenericResolverBehavior]
    public class IngestedDataDA : IIngestedDataDA
    {
        private IFeedReposository m_FeedRepository;
        private string m_ThisName = "IngestedDataDA";

        public IngestedDataDA()
        {
            m_FeedRepository = new FeedReposositoryAzStor();
        }

        #region IIngestedDataDA Members

        void IIngestedDataDA.SaveTestData(InProcessFeedMsg msg)
        {
            ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(m_ThisName + ".SaveTestData(): Entered:");
            m_FeedRepository.SaveTestData(msg);
        }
        #endregion
    }
}
