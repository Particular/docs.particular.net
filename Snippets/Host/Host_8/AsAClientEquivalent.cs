using NServiceBus;
using NServiceBus.Features;

public class AsAClientEquivalent
{
    public AsAClientEquivalent()
    {
        #region AsAClientEquivalent

        var endpointConfiguration = new EndpointConfiguration("MyEndpointName");
        endpointConfiguration.PurgeOnStartup(true);

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.Transactions(TransportTransactionMode.None);

        endpointConfiguration.Recoverability().Delayed(cfg => cfg.NumberOfRetries(0));
        endpointConfiguration.DisableFeature<TimeoutManager>();

        #endregion
    }
}