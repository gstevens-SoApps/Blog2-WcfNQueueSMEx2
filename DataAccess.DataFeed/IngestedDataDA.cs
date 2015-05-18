using GS.Contract.DataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Contract.DataFeed.Server;

namespace GS.DataAccess.IngestedDataDA
{
    public class IngestedDataDA : IIngestedDataDA
    {
        bool IIngestedDataDA.SaveTestMessage(FeedProcessingMsg msg)
        {
            throw new NotImplementedException();
        }

        bool IIngestedDataDA.SaveBadMessage(FeedProcessingMsg msg)
        {
            throw new NotImplementedException();
        }
    }
}
