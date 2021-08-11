using EPiServer.Forms.Core.Feed.Internal;
using EPiServer.ServiceLocation;
using System.Collections.Generic;

namespace MarketingAutomation.ODP
{
    public class FeedProvider : IFeedProvider
    {
        public IEnumerable<IFeed> GetFeeds() =>
            ServiceLocator.Current.GetAllInstances<IFeed>();
    }
}