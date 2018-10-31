namespace Contracts
{
    using NServiceBus;

    class ServiceControlPublisherConfig
    {
        ServiceControlPublisherConfig(EndpointConfiguration endpointConfiguration)
        {
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

            #region ServiceControlPublisherConfig

            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(ServiceControl.Contracts.MessageFailed).Assembly, "Particular.ServiceControl");

            #endregion
        }
    }
}