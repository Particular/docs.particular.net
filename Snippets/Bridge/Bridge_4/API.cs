using NServiceBus;

public class API
{
    public void Configuration()
    {
        var connectionString = string.Empty;
        var bridgeConfiguration = new BridgeConfiguration();

        #region bridgeconfiguration

        var msmq = new BridgeTransport(new MsmqTransport());
        var asb = new BridgeTransport(new AzureServiceBusTransport(connectionString));

        msmq.HasEndpoint("Sales");
        asb.HasEndpoint("Billing");

        bridgeConfiguration.AddTransport(msmq);
        bridgeConfiguration.AddTransport(asb);

        #endregion
    }
}