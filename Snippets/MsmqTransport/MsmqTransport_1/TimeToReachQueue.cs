using System;
using NServiceBus;

class TimeToReachQueue
{
    TimeToReachQueue(EndpointConfiguration endpointConfiguration)
    {
        #region time-to-reach-queue

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.TimeToReachQueue(TimeSpan.FromMinutes(15));

        #endregion
    }
}
