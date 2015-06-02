/*
GS.DataAccess.DataFeed.InProcessFeedMsg
  
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
using System;
using System.Runtime.Serialization;

namespace GS.DataAccess.Common
{
    [DataContract(Namespace = "GeorgeStevens/SoApps/5/15")]
    public class InProcessFeedMsg
    {
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public DateTime MessageReceivedDateTime { get; set; }
        [DataMember]
        public long QueueLength { get; set; }
        [DataMember]
        public SbMessage MessageInProcess { get; set; }
    }
}
