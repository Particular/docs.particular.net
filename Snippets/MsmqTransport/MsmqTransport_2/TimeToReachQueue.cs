using System;
using NServiceBus;

class TimeToReachQueue
{
    TimeToReachQueue(EndpointConfiguration endpointConfiguration)
    {
        #region time-to-reach-queue

        var transport = new MsmqTransport
        {
            TimeToReachQueue = TimeSpan.FromMinutes(15)
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
