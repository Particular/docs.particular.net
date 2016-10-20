using NServiceBus;

class MyClass
{
    public void Foo()
    {
        #region SagaAudit_Configure_ServiceControl
        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.SagaPlugin("ServiceControl_Queue");
        #endregion
    }
}