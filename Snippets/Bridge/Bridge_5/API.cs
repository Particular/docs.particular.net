using NServiceBus;

public class API
{
    public void Configuration()
    {
        var connectionString = string.Empty;

        #region bridgeconfiguration

        var bridgeConfiguration = new BridgeConfiguration();

        var msmq = new BridgeTransport(new MsmqTransport());
        var asb = new BridgeTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));

        msmq.HasEndpoint("Sales");
        asb.HasEndpoint("Billing");

        bridgeConfiguration.AddTransport(msmq);
        bridgeConfiguration.AddTransport(asb);

        #endregion
    }
}
