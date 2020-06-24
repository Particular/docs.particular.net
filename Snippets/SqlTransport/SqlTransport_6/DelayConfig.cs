using System;
using NServiceBus;

class DelayConfig
{
    void ConfigurePeekDelay(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-config-delay

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.WithPeekDelay(TimeSpan.FromSeconds(5));

        #endregion
    }
}
