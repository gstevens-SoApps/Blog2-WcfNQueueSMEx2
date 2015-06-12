/*
GS.Proxy.Admin.FeedAdminClient
  
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
using System;
using System.ServiceModel;

namespace GS.Proxy.Admin
{
    public class FeedAdminClient : ClientBase<IFeedAdmin>, IFeedAdmin
    {
        public FeedAdminClient(string endpointName)
            : base(endpointName)
        { }

        public DataFeedStatistics PresentFeedComponentInfo(string componentName)
        {
            // "Programming WCF Services", 3rd Edition by Juval Lowy
            // pp 258 - 259 recommends the form of implementing proxies like below.
            try
            {
                DataFeedStatistics stats = Channel.PresentFeedComponentInfo(componentName);
                return stats;
            }
            catch (Exception)
            {
                Abort();
                throw;
            }
        }
    }
}
