using System;
using NServiceBus;

class Upgrade3to4
{
    void MaxTTL(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_MaxTTL

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MaxTimeToLive(TimeSpan.FromDays(10));

        #endregion
    }
}