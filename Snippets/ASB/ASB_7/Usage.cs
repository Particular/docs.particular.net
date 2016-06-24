using System;
using System.Reflection;
using NServiceBus;
using NServiceBus.AzureServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://{namespace}.servicebus.windows.net/;SharedAccessKeyName={keyname};SharedAccessKey={keyvalue}");

        #endregion
    }

    void SettingConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region setting_asb_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://{namespace}.servicebus.windows.net/;SharedAccessKeyName={name};SharedAccessKey={key}");

        #endregion
    }

    void SettingQueueProperties(EndpointConfiguration endpointConfiguration)
    {
        #region setting_queue_properties

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var queueSettings = transport.Queues();
        queueSettings.LockDuration(TimeSpan.FromMinutes(1));

        #endregion
    }

    void SettingTopicProperties(EndpointConfiguration endpointConfiguration)
    {
        #region setting_topic_properties

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topicSettings = transport.Topics();
        topicSettings.MaxSizeInMegabytes(SizeInMegabytes.Size5120);

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

    class MyMessage
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
        var receiverSettings = transport.MessageReceivers();
        receiverSettings.AutoRenewTimeout(maximumProcessingTime);

        #endregion
    }

    void ForwardDeadLettersConditional(EndpointConfiguration endpointConfiguration)
    {
        #region forward-deadletter-conditional-queue
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        var queueSettings = transport.Queues();
        queueSettings.ForwardDeadLetteredMessagesTo(entityname => entityname == "yourqueue", "errorqueue");

        #endregion
    }

    void SwappingSanitizationStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-sanitization-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var sanitizationSettings = transport.Sanitization();
        sanitizationSettings.UseStrategy<MySanitizationStrategy>();

        #endregion
    }

    void SwappingCompositionStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-composition-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var compositionSettings = transport.Composition();
        compositionSettings.UseStrategy<MyCompositionStrategy>();

        #endregion
    }

    void SwappingPartitioningStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-namespace-partitioning-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var partitioningSettings = transport.NamespacePartitioning();
        partitioningSettings.UseStrategy<MyNamespacePartitioningStrategy>();

        #endregion
    }

    void SwappingIndividualizationStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region swap-individualization-strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var individualizationSettings = transport.Individualization();
        individualizationSettings.UseStrategy<MyIndividualizationStrategy>();

        #endregion
    }

}

