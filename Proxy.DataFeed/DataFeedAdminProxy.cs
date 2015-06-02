using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GS.Contract.DataFeed;

namespace GS.Proxy.DataFeed
{
    public class DataFeedAdminClient : ClientBase<IFeedAdmin>, IFeedAdmin
    {
        public DataFeedAdminClient(string endpointName)
            : base(endpointName)
        {}

        public DataFeedStatistics PresentFeedComponentInfo(string componentName)
        {
            // "Programming WCF Services", 3rd Edition by Juval Lowy
            // pp 258 - 259 recommends the form of implementing proxies like below.
            try
            {
                DataFeedStatistics stats = Channel.PresentFeedComponentInfo(componentName);
                return stats;
            }
            catch (Exception)
            {
                Abort();
                throw;
            }
        }
    }
}
