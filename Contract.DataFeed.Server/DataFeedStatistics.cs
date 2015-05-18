using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GS.Contract.DataFeed.Server
{
    [DataContract(Namespace = "GeorgeStevens/SoApps/5/15")]
    public class DataFeedStatistics
    {
        [DataMember]
        public int DataFeedId { get; set; }

        [DataMember]
        public DateTime StatsCollectionDateTime;

        [DataMember]
        public int QueueLength { get; set; }

        [DataMember]
        public int DeadLetterQueueLength { get; set; }
    }
}
