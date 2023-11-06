using System;
using Azure.Identity;
using NServiceBus;

internal class Upgrade2to3
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        var transport = new AzureServiceBusTransport("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #region 2to3-token-credentials

        var transportWithTokenCredentials = new AzureServiceBusTransport("[NAMESPACE].servicebus.windows.net", new DefaultAzureCredential());
        endpointConfiguration.UseTransport(transportWithTokenCredentials);

        #endregion

        #region 2to3-custom-auto-lock-renewal

        transport.MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(10);

        #endregion
    }
}

