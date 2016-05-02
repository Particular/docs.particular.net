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

    void SettingConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region setting_asb_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://{namespace}.servicebus.windows.net/;SharedAccessKeyName={sasname};SharedAccessKey={saskey}");

        #endregion
    }

    void SettingQueueProperties(EndpointConfiguration endpointConfiguration)
    {
        #region setting_queue_properties

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.Queues().LockDuration(TimeSpan.FromMinutes(1));

        #endregion
    }

    void SettingTopicProperties(EndpointConfiguration endpointConfiguration)
    {
        #region setting_topic_properties

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.Topics().MaxSizeInMegabytes(SizeInMegabytes.Size5120);

        #endregion
    }

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
        var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
        topology.RegisterPublisherForAssembly("publisherName", messagesAssembly);

        #endregion
    }

    void PublisherNamesMappingUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        #region publisher_names_mapping_upgrade_guide

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topology = transport.UseTopology<EndpointOrientedTopology>();

        topology.RegisterPublisherForType("publisherName", typeof(MyMessage));
        // or
        var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
        topology.RegisterPublisherForAssembly("publisherName", messagesAssembly);

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

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.MessageReceivers().AutoRenewTimeout(maximumProcessingTime);

        #endregion
    }

    void ForwardDeadLettersConditional(EndpointConfiguration endpointConfiguration)
    {
        #region forward-deadletter-conditional-queue
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        transport.Queues().ForwardDeadLetteredMessagesTo(entityname => entityname == "yourqueue", "errorqueue");


        #endregion
    }
}