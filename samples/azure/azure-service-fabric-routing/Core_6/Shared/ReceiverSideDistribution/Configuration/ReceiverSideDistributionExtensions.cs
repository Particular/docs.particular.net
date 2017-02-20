using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

namespace Shared
{
    public static class ReceiverSideDistributionExtensions
    {
        public static void EnableReceiverSideDistribution(this EndpointConfiguration configuration, HashSet<string> discriminators, Func<object,string> mapper, Action<string> logger = null)
        {
            configuration.GetSettings().Set(ReceiverSideDistribution.Discriminators, discriminators);
            configuration.GetSettings().Set(ReceiverSideDistribution.Mapper, mapper);
            configuration.GetSettings().Set(ReceiverSideDistribution.Logger, logger);
            configuration.EnableFeature<ReceiverSideDistribution>();
        }
    }
}