using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GS.Contract.DataFeed
{
    [DataContract(Namespace = "GeorgeStevens/SoApps/5/15")]
    public class FeedProcessingMsg
    {
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public DateTime MessageReceivedDateTime { get; set; }
        [DataMember]
        public SbMessage TheMessage { get; set; }
    }
}
