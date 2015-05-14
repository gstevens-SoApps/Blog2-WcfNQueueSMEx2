/*
Shared.DataFeedsServiceProxy
  
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
using ServiceModelEx.ServiceBus;
using System;

namespace Shared
{
    public class DataFeedsClient : QueuedServiceBusClient<IDataFeeds>, IDataFeeds
    {
        public DataFeedsClient(string endpointName, string sessionId = null)
            : base(endpointName, sessionId)
        {
        }

        public void IngestTestData(TestMessage msg)
        {
            // "Programming WCF Services", 3rd Edition by Juval Lowy
            // pp 258 - 259 recommends the form of implementing proxies like below.
            try
            {
                Channel.IngestTestData(msg);
            }
            catch (Exception ex)
            {
                Abort();
                throw;
            }
        }
    }
}
