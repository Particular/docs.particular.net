using NServiceBus;
using ServiceControl.Features;

class MyClass
{
    public void Foo()
    {
        #region SagaAudit_Configure_ServiceControl

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.SagaPlugin(
            serviceControlQueue: "particular.servicecontrol@machine");

        #endregion
    }

    public void Foo2()
    {
        #region SagaAudit_disable

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.DisableFeature<SagaAudit>();

        #endregion
    }

    public void Foo3()
    {
        var isSagaPluginEnabled = true;

        #region SagaAudit_Configurable

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        if (isSagaPluginEnabled)
        {
            endpointConfiguration.SagaPlugin(
            serviceControlQueue: "particular.servicecontrol@machine");
        }

        #endregion
    }
}
