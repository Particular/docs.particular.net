
namespace Messages
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            // NServiceBus provides multiple durable storage options, including SQL Server, RavenDB, and Azure Storage Persistence. 
            // Refer to the documentation for more details on specific options.
            endpointConfiguration.UsePersistence<PLEASE_SELECT_ONE>();
        }
    }
}
