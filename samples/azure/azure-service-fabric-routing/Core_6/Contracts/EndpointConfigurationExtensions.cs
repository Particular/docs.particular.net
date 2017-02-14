using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

namespace Contracts
{
    public static class EndpointConfigurationExtensions
    {
        public static void EnableReceiverSideDistribution(this EndpointConfiguration configuration, HashSet<string> discriminators)
        {
            configuration.GetSettings().Set("ReceiverSideDistribution.Discriminators", discriminators);
            configuration.EnableFeature<ReceiverSideDistribution>();
        }
    }
}