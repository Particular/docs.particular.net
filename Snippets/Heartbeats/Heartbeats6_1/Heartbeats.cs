using NServiceBus;

class MyClass
{
    public void Foo4()
    {

        #region Heartbeats_disable

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.DisableFeature<ServiceControl.Plugin.Nsb6.Heartbeat.Heartbeats>();

        #endregion
    }
}