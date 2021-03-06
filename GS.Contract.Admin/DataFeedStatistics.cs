﻿/*
GS.Contract.Admin.DataFeedStatistics
  
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

using System;
using System.Runtime.Serialization;

namespace GS.Contract.Admin
{
    [DataContract(Namespace = "GeorgeStevens/SoApps/5/15")]
    public class DataFeedStatistics
    {
        [DataMember]
        public string FeedComponentName { get; set; }
        [DataMember]
        public DateTime StatsCollectionDateTime;
        // TODO George 6-1-15.  This needs to be generalized to support more than queues.
        [DataMember]
        public long QueueLength { get; set; }
        [DataMember]
        public long DeadLetterQueueLength { get; set; }
    }
}
