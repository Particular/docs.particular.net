namespace ASB_7
{
    using System.Reflection;
    using NServiceBus;
    using NServiceBus.AzureServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region AzureServiceBusTransportWithAzure

            endpointConfiguration.UseTransport<AzureServiceBusTransport>();

            #endregion
        }

        #region AzureServiceBusTransportWithAzureHost 7

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            }
        }

        #endregion

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

        void PublisherNamesMappingUpgradeGuide(EndpointConfiguration endpointConfiguration)
        {
            #region publisher_names_mapping_upgrade_guide

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseTopology<EndpointOrientedTopology>()
                .RegisterPublisherForType("publisherName", typeof(MyMessage));
            // or
            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseTopology<EndpointOrientedTopology>()
                .RegisterPublisherForAssembly("publisherName", Assembly.LoadFrom("path/to/the/assembly/that/contains/messages"));

            #endregion
        }

        private class MyMessage
        {
        }

        void TopologySelectionUpgradeGuide(EndpointConfiguration endpointConfiguration)
        {
            #region topology-selection-upgrade-guide

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseTopology<ForwardingTopology>();
            // or
            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .UseTopology<EndpointOrientedTopology>();

            #endregion
        }
    }
}
