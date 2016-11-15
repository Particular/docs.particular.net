using System;
using NServiceBus;
using ServiceControl.Features;

class MyClass
{
    public void Configure_ServiceControl()
    {
        #region Heartbeats_Configure_ServiceControl

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin(
            serviceControlQueue: "ServiceControl_Queue");

        #endregion
    }

    public void Interval()
    {
        #region Heartbeats_interval

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin(
            serviceControlQueue: "ServiceControl_Queue",
            frequency: TimeSpan.FromMinutes(2));

        #endregion
    }

    public void Ttl()
    {
        #region Heartbeats_ttl

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin(
            serviceControlQueue: "ServiceControl_Queue",
            frequency: TimeSpan.FromSeconds(30),
            timeToLive: TimeSpan.FromMinutes(3));

        #endregion
    }

    public void Disable()
    {
        #region Heartbeats_disable

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.DisableFeature<Heartbeats>();

        #endregion
    }
}
