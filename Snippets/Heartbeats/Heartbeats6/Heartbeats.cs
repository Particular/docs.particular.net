using System;
using NServiceBus;

class MyClass
{
    public void Foo()
    {
        #region Heartbeats_Configure_ServiceControl [2,3)

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin("ServiceControl_Queue");

        #endregion
    }

    public void Foo2()
    {
        #region Heartbeats_interval [2,3)

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin("ServiceControl_Queue", frequency: TimeSpan.FromMinutes(2));

        #endregion
    }

    public void Foo3()
    {

        #region Heartbeats_ttl [2,3)

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.HeartbeatPlugin("ServiceControl_Queue", TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(3));

        #endregion
    }
}