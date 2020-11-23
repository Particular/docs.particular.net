namespace Host_7
{
    #region host-endpoint-config-sample
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            //TODO: NServiceBus provides multiple durable storage options, including SQL Server, RavenDB, and Azure Table Persistence.
            // Refer to the documentation for more details on specific options.
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            // NServiceBus will move messages that fail repeatedly to a separate "error" queue. It is recommended
            // to start with a shared error queue for all endpoints for easy integration with ServiceControl.
            endpointConfiguration.SendFailedMessagesTo("error");
            // NServiceBus will store a copy of each successfully process message in a separate "audit" queue. It is recommended
            // to start with a shared audit queue for all endpoints for easy integration with ServiceControl.
            endpointConfiguration.AuditProcessedMessagesTo("audit");
        }
    }
    #endregion
}