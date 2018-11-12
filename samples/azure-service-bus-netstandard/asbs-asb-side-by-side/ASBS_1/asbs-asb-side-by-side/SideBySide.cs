//  Edit The Project And See The Alias Definition
#region transport-asb-alias-definition
extern alias LegacyASB;
#endregion

using NServiceBus;
using LegacyASB::NServiceBus;

public class SideBySide
{
    public SideBySide()
    {
        var configurationASB = new EndpointConfiguration("ASB");

        #region transport-asb-alias-usage

        var legacyASB = configurationASB.UseTransport<LegacyASB::NServiceBus.AzureServiceBusTransport>();

        #endregion

        legacyASB.Queues().EnableBatchedOperations(true);

        var configurationASBS = new EndpointConfiguration("ASBS");
        var transportASB = configurationASBS.UseTransport<NServiceBus.AzureServiceBusTransport>();

        transportASB.PrefetchMultiplier(1);
    }
}