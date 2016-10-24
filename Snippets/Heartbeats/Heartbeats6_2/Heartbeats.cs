using System;
using NServiceBus;
using ServiceControl.Features;

class MyClass
{
    public void Foo()
    {
        #region Heartbeats_Configure_ServiceControl

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin("ServiceControl_Queue");

        #endregion
    }

    public void Foo2()
    {
        #region Heartbeats_interval

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin("ServiceControl_Queue", frequency: TimeSpan.FromMinutes(2));

        #endregion
    }

    public void Foo3()
    {
        #region Heartbeats_ttl

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin("ServiceControl_Queue", TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(3));

        #endregion
    }

    public void Foo4()
    {
        #region Heartbeats_disable

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.DisableFeature<Heartbeats>();

        #endregion
    }
}
