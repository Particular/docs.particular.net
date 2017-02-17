using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

namespace Shared
{
    public static class ReceiverSideDistributionExtensions
    {
        public static ReceiverSideDistributionOptions EnableReceiverSideDistribution(this EndpointConfiguration configuration, HashSet<string> discriminators, Func<object,string> mapper, Action<string> logger = null)
        {
            var options = new ReceiverSideDistributionOptions();
            configuration.GetSettings().Set("ReceiverSideDistribution.Options", options);

            configuration.GetSettings().Set("ReceiverSideDistribution.Discriminators", discriminators);
            configuration.GetSettings().Set("ReceiverSideDistribution.Mapper", mapper);
            configuration.GetSettings().Set("ReceiverSideDistribution.Logger", logger);
            configuration.EnableFeature<ReceiverSideDistribution>();

            return options;
        }

        public static void TrustIncomingRepliesWithoutHeaderOrContext(this ReceiverSideDistributionOptions configuration)
        {
            configuration.TrustReplies = true;
        }
    }
}