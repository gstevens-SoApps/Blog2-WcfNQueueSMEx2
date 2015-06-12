/*
GS.Manager.Admin.AdminManager
  
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
using GS.DataAccess.Common;
using GS.DataAccess.FeedAdmin;
using GS.Engine.Common;
using GS.iFX.TestUI;
using ServiceModelEx;
using System.ServiceModel;

namespace GS.Manager.Admin
{
    [GenericResolverBehavior]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AdminManager : IFeedAdmin
    {
        private string m_ThisName = "AdminManager";

        #region IFeedAdmin Members

        DataFeedStatistics IFeedAdmin.PresentFeedComponentInfo(string componentName)
        {
            ConsoleNTraceHelpers.DisplayInfoToConsoleNTrace(m_ThisName + ".PresentFeedComponentInfo(): Entered:");

            // Check validity of all requests.
            IAdminValidityEngine validityEngProxy = InProcFactory.CreateInstance<ValidityEngine, IAdminValidityEngine>();

            validityEngProxy.IsPresentFeedComponentInfoRequestValid(componentName);
            InProcFactory.CloseProxy(validityEngProxy);

            // Retrieve Feed Component info
            DataFeedStatistics stats = null;
            IFeedAdminDA feedAdminDaProxy = InProcFactory.CreateInstance<AdminDA, IFeedAdminDA>();
            stats = feedAdminDaProxy.GetFeedStatistics(componentName);
            InProcFactory.CloseProxy(feedAdminDaProxy);
            
            return stats;
        }

        #endregion
    }
}
