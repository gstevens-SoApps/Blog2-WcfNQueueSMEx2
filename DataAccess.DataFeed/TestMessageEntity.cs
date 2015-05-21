/*
GS.DataAccess.DataFeed.TestMessageEntity
  
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

using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace GS.DataAccess.DataFeed
{
    public class TestMessageEntity : TableEntity
    {
        // TODO Remove the business logic in the ctor so that this class can be put
        // in Infrastructure.
        public TestMessageEntity(int sourceGroupId, int sourceId, DateTime messageSendDateTime)
        {
            PartitionKey = string.Format("{0:d4}-", sourceGroupId) + string.Format("{0:d4}", sourceId);
          
            // Ensure that time zone does NOT get appended to the string by
            // setting kind to Unspecified. "o" is used to include milliseconds since
            // without them one easily encounters insert exceptions due to duplicate row keys.
            DateTime unspecifiedDt = 
                DateTime.SpecifyKind(messageSendDateTime, DateTimeKind.Unspecified);
            RowKey = unspecifiedDt.ToString("o");
        }

        public TestMessageEntity()
        { }
        // From SbMessage
        public Guid MessageId { get; set; }
        public DateTime MessageSendDateTime { get; set; }

        // From info on server
        public DateTime MessageReceivedDateTime { get; set; }
        //public TimeSpan ElapsedTime { get; set; }
        public string ElapsedTimeString { get; set; } // TimeSpan not supported in Table Storage
        public string ErrorText { get; set; }

        // From TestMessage
        public string MsgBody { get; set; }
        public int SourceMsgSeqNumber { get; set; }
        public int SourceId { get; set; }
        public int SourceGroupId { get; set; }
    }
}
