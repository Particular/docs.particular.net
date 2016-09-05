using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport.AzureServiceBus;

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
        var topology = transport.UseTopology<EndpointOrientedTopology>();
        topology.RegisterPublisher(typeof(MyMessage), "publisherName");

        #endregion
    }

    void PublisherNamesMappingByAssembly(EndpointConfiguration endpointConfiguration)
    {
        #region publisher_names_mapping_by_assembly

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topology = transport.UseTopology<EndpointOrientedTopology>();
        var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
        topology.RegisterPublisher(messagesAssembly, "publisherName");

        #endregion
    }

    void PublisherNamesMappingUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        #region publisher_names_mapping_upgrade_guide

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var topology = transport.UseTopology<EndpointOrientedTopology>();

        topology.RegisterPublisher(typeof(MyMessage), "publisherName");
        // or
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
            condition: entityname => entityname == "yourqueue",
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

    void IncomingBrokeredMessageBody(EndpointConfiguration endpointConfiguration)
    {
        #region asb-incoming-message-convention

        var transportConfig = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transportConfig.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);
        // or transportConfig.UseBrokeredMessageToIncomingMessageConverter<CustomIncomingMessageConversion>();

        #endregion
    }

    void OutgoingBrokeredMessageBody(EndpointConfiguration endpointConfiguration)
    {
        #region asb-outgoing-message-convention

        var transportConfig = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transportConfig.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);
        // or transportConfig.UseOutgoingMessageToBrokeredMessageConverter<CustomOutgoingMessageConversion>();

        #endregion
    }


    public class CustomIncomingMessageConversion : IConvertBrokeredMessagesToIncomingMessages
    {
        public IncomingMessageDetails Convert(BrokeredMessage brokeredMessage)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomOutgoingMessageConversion : IConvertOutgoingMessagesToBrokeredMessages
    {
        public IEnumerable<BrokeredMessage> Convert(IEnumerable<BatchedOperation> outgoingOperations, RoutingOptions routingOptions)
        {
            throw new NotImplementedException();
        }
    }
}
