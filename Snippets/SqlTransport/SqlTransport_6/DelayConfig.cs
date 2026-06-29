using System;
using NServiceBus;

class DelayConfig
{
    void ConfigurePeekDelay(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-queue-peeker-config-delay

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.QueuePeekerOptions(delay: TimeSpan.FromSeconds(5));

        #endregion
    }
}
