using System;
using System.Reflection;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }

    void SettingConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region setting_asb_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }

    void SettingQueueProperties(EndpointConfiguration endpointConfiguration)
    {
        #region setting_queue_properties

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var queues = transport.Queues();
        queues.LockDuration(TimeSpan.FromMinutes(1));

        #endregion
    }

    void SettingTopicProperties(EndpointConfiguration endpointConfiguration)
    {
        #region setting_topic_properties

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topics = transport.Topics();
        topics.MaxSizeInMegabytes(SizeInMegabytes.Size5120);

        #endregion
    }

    void PublisherNamesMappingByMessageType(EndpointConfiguration endpointConfiguration)
    {
        #region publisher_names_mapping_by_message_type

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topology = transport.UseEndpointOrientedTopology();
        topology.RegisterPublisher(typeof(MyMessage), "publisherName");

        #endregion
    }

    void PublisherNamesMappingByAssembly(EndpointConfiguration endpointConfiguration)
    {
        #region publisher_names_mapping_by_assembly

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topology = transport.UseEndpointOrientedTopology();
        var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
        topology.RegisterPublisher(messagesAssembly, "publisherName");

        #endregion
    }

    void PublisherNamesMappingUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        #region publisher_names_mapping_upgrade_guide

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topology = transport.UseEndpointOrientedTopology();

        topology.RegisterPublisher(typeof(MyMessage), "publisherName");
        // OR
        var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
        topology.RegisterPublisher(messagesAssembly, "publisherName");

        #endregion
    }

    class MyMessage
    {
    }

    void TopologySelectionUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        #region topology-selection-upgrade-guide

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        transport.UseForwardingTopology();
        // OR
        transport.UseEndpointOrientedTopology();

        #endregion
    }

    void AutoLockRenewal(EndpointConfiguration endpointConfiguration)
    {
        var maximumProcessingTime = TimeSpan.FromMinutes(5);

        #region asb-auto-lock-renewal

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var receivers = transport.MessageReceivers();
        receivers.AutoRenewTimeout(maximumProcessingTime);

        #endregion
    }

    void ForwardDeadLettersConditional(EndpointConfiguration endpointConfiguration)
    {
        #region forward-deadletter-conditional-queue

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        var queues = transport.Queues();
        queues.ForwardDeadLetteredMessagesTo(
            condition: entityName =>
            {
                return entityName == "yourqueue";
            },
            forwardDeadLetteredMessagesTo: "errorqueue");

        #endregion
    }

    void SwappingSanitizationStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-sanitization-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var sanitization = transport.Sanitization();
        sanitization.UseStrategy<MySanitizationStrategy>();

        #endregion
    }

    void SwappingCompositionStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-composition-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var composition = transport.Composition();
        composition.UseStrategy<MyCompositionStrategy>();

        #endregion
    }

    void SwappingPartitioningStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-namespace-partitioning-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var partitioning = transport.NamespacePartitioning();
        partitioning.UseStrategy<MyNamespacePartitioningStrategy>();

        #endregion
    }

    void SwappingIndividualizationStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-individualization-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var individualization = transport.Individualization();
        individualization.UseStrategy<MyIndividualizationStrategy>();

        #endregion
    }

    void Serializer(EndpointConfiguration endpointConfiguration)
    {
        #region asb-serializer

        endpointConfiguration.UseSerialization<XmlSerializer>();

        #endregion
    }
}