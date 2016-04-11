namespace Snippets6.Azure.Transports.AzureServiceBus
{
    using System.Reflection;
    using NServiceBus;
    using NServiceBus.AzureServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region AzureServiceBusTransportWithAzure 7

            endpointConfiguration.UseTransport<AzureServiceBusTransport>();

            #endregion
        }

        //TODO: fix when we split azure
        /**
        #region AzureServiceBusTransportWithAzureHost 7

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            }
        }

        #endregion
        **/
        
        void PublisherNamesMappingByMessageType(EndpointConfiguration endpointConfiguration)
        {
            #region publisher_names_mapping_by_message_type

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseTopology<EndpointOrientedTopology>()
                .RegisterPublisherForType("publisherName", typeof(MyMessage));

            #endregion
        }

        void PublisherNamesMappingByAssembly(EndpointConfiguration endpointConfiguration)
        {
            #region publisher_names_mapping_by_assembly

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseTopology<EndpointOrientedTopology>()
                .RegisterPublisherForAssembly("publisherName", Assembly.LoadFrom("path/to/the/assembly/that/contains/messages"));

            #endregion
        }

        private class MyMessage
        {
        }
    }
}
