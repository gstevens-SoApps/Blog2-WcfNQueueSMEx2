﻿/*
GS.Proxy.DataFeed.DataFeedsClient
  
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
using ServiceModelEx.ServiceBus;
using System;

namespace GS.Proxy.DataFeed
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
            catch (Exception)
            {
                Abort();
                throw;
            }
        }

        // TODO 5-17-15 George.  The NetMessagingBinding requires
        // OneWay=true. Move this operation into another service contract
        // that uses a TCP binding, not NetMessagingBinding.
        //public DataFeedStatistics CollectDataFeedStatistics()
        //{
        //    DataFeedStatistics stats = null;
        //    try
        //    {
        //        stats = Channel.CollectDataFeedStatistics();
        //    }
        //    catch (Exception ex)
        //    {
        //        Abort();
        //        throw;
        //    }
        //    return stats;
        //}
    }
}
