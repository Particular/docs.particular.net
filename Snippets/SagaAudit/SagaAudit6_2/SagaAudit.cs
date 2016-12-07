using NServiceBus;
using ServiceControl.Features;

class MyClass
{
    public void Foo()
    {
        #region SagaAudit_Configure_ServiceControl

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.SagaPlugin(
            serviceControlQueue: "ServiceControl_Queue");

        #endregion
    }

    public void Foo2()
    {
        #region SagaAudit_disable

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.DisableFeature<SagaAudit>();

        #endregion
    }
}