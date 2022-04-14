using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport;

public class API
{
    public void Configuration()
    {
        var connectionString = string.Empty;
        var bridgeConfiguration = new BridgeConfiguration();

        #region bridgeconfiguration

        var msmq = new BridgeTransportConfiguration(new MsmqTransport());
        var asb = new BridgeTransportConfiguration(new AzureServiceBusTransport(connectionString));

        msmq.HasEndpoint("Sales");
        asb.HasEndpoint("Billing");

        bridgeConfiguration.AddTransport(msmq);
        bridgeConfiguration.AddTransport(asb);

        #endregion
    }
}