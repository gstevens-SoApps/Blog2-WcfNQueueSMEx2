using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GS.Contract.DataFeed.Server
{
    public interface IFeedValidityEngine
    {
        FeedProcessingMsg CheckValidity(FeedProcessingMsg msg);
    }
}
