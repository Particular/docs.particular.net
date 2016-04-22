namespace ASB_7
{
    using System;
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

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var topology = transport.UseTopology<EndpointOrientedTopology>();
            topology.RegisterPublisherForType("publisherName", typeof(MyMessage));

            #endregion
        }

        void PublisherNamesMappingByAssembly(EndpointConfiguration endpointConfiguration)
        {
            #region publisher_names_mapping_by_assembly

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var topology = transport.UseTopology<EndpointOrientedTopology>();
            topology.RegisterPublisherForAssembly("publisherName", Assembly.LoadFrom("path/to/assembly/containing/messages"));

            #endregion
        }

        void PublisherNamesMappingUpgradeGuide(EndpointConfiguration endpointConfiguration)
        {
            #region publisher_names_mapping_upgrade_guide

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var topology = transport.UseTopology<EndpointOrientedTopology>();

            topology.RegisterPublisherForType("publisherName", typeof(MyMessage));
            // or
            topology.RegisterPublisherForAssembly("publisherName", Assembly.LoadFrom("path/to/assembly/containing/messages"));

            #endregion
        }

        private class MyMessage
        {
        }

        void TopologySelectionUpgradeGuide(EndpointConfiguration endpointConfiguration)
        {
            #region topology-selection-upgrade-guide
            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

            transport.UseTopology<ForwardingTopology>();
            // or
            transport.UseTopology<EndpointOrientedTopology>();

            #endregion
        }

        void AutoLockRenewal(EndpointConfiguration endpointConfiguration)
        {
            var maximumProcessingTime = TimeSpan.FromMinutes(5);

            #region asb-auto-lock-renewal

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .MessageReceivers().AutoRenewTimeout(maximumProcessingTime);

            #endregion
        }
    }
}
