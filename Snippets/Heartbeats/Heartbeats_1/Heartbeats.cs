using System;
using NServiceBus;

class MyClass
{
    public void Configure_ServiceControl()
    {
        #region HeartbeatsNew_Enable

        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: "ServiceControl_Queue",
            frequency: TimeSpan.FromSeconds(15),
            timeToLive: TimeSpan.FromSeconds(30));

        #endregion
    }
}
