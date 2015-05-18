using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Contract.DataFeed.Server
{
    public interface IIngestedDataDA
    {
        bool SaveTestMessage(FeedProcessingMsg msg);
        bool SaveBadMessage(FeedProcessingMsg msg);
    }
}
