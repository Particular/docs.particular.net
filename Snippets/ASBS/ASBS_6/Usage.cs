using System;
using System.IO;
using System.Text.Json;
using Azure.Identity;
using NServiceBus;
using NServiceBus.Transport;
using NServiceBus.Transport.AzureServiceBus;
using Shipping;
using Azure.Messaging.ServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region azure-service-bus-for-dotnet-standard

        var transport = new AzureServiceBusTransport("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]", TopicTopology.Default);
        endpointConfiguration.UseTransport(transport);

        #endregion

        #region token-credentials

        var transportWithTokenCredentials = new AzureServiceBusTransport("[NAMESPACE].servicebus.windows.net", new DefaultAzureCredential(), TopicTopology.Default);
        endpointConfiguration.UseTransport(transportWithTokenCredentials);

        #endregion

        #region custom-prefetch-multiplier

        transport.PrefetchMultiplier = 3;

        #endregion

        #region custom-prefetch-count

        transport.PrefetchCount = 100;

        #endregion

        #region custom-auto-lock-renewal

        transport.MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(10);

        #endregion

#pragma warning disable CS0618 // Type or member is obsolete
        #region asb-sanitization-compatibility

        var migrationTopology = TopicTopology.MigrateFromSingleDefaultTopic();
        migrationTopology.OverrideSubscriptionNameFor("QueueName", "ShortenedSubscriptionName");

        migrationTopology.EventToMigrate<MyEvent>("ShortenedRuleName");

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete

        #region asb-options-validation-disable

        transport.Topology.OptionsValidator = new TopologyOptionsDisableValidationValidator();

        #endregion

        #region asb-options-options-loading

        using var stream = File.OpenRead("topology-options.json");
        var options = JsonSerializer.Deserialize<TopologyOptions>(stream, TopologyOptionsSerializationContext.Default.Options);
        var jsonTopology = TopicTopology.FromOptions(options);

        #endregion

        var topology = TopicTopology.Default;
        #region asb-interface-based-inheritance
        topology.SubscribeTo<IOrderStatusChanged>("Shipping.OrderAccepted");
        #endregion

        #region asb-interface-based-inheritance-declined
        topology.SubscribeTo<IOrderStatusChanged>("Shipping.OrderAccepted");
        topology.SubscribeTo<IOrderStatusChanged>("Shipping.OrderDeclined");
        #endregion

        #region asb-versioning-subscriber-mapping
        topology.SubscribeTo<IOrderAccepted>("Shipping.OrderAccepted");
        topology.SubscribeTo<IOrderAccepted>("Shipping.OrderAcceptedV2");
        #endregion

        #region asb-interface-based-inheritance-publisher
        topology.PublishTo<OrderAccepted>("Shipping.IOrderStatusChanged");
        topology.PublishTo<OrderDeclined>("Shipping.IOrderStatusChanged");
        #endregion

        #region asb-versioning-publisher-mapping

        topology.PublishTo<OrderAccepted>("Shipping.OrderAccepted");
        topology.PublishTo<OrderAcceptedV2>("Shipping.OrderAccepted");
        #endregion

        #region asb-versioning-publisher-customization
        transport.OutgoingNativeMessageCustomization = (operation, message) =>
        {
            if (operation is MulticastTransportOperation multicastTransportOperation)
            {
                // Subject is used for demonstration purposes only, choose a property that fits your scenario
                message.Subject = multicastTransportOperation.MessageType.FullName;
            }
        };
        #endregion

        #region azure-service-bus-usewebsockets
        transport.UseWebSockets = true;
        #endregion

        #region azure-service-bus-websockets-proxy
        transport.WebProxy = new System.Net.WebProxy("http://myproxy:8080");
        #endregion

        #region azure-service-bus-TimeToWaitBeforeTriggeringCircuitBreaker
        transport.TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(2);
        #endregion

        #region azure-service-bus-RetryPolicyOptions
        var azureAsbRetryOptions = new Azure.Messaging.ServiceBus.ServiceBusRetryOptions 
        { 
            Mode = Azure.Messaging.ServiceBus.ServiceBusRetryMode.Exponential,
            MaxRetries = 5,
            Delay = TimeSpan.FromSeconds(0.8),
            MaxDelay = TimeSpan.FromSeconds(15)
        };
        transport.RetryPolicyOptions = azureAsbRetryOptions;
        #endregion

        #region azure-service-bus-entitymaximumsize
        transport.EntityMaximumSize = 5;
        #endregion

        #region azure-service-bus-enablepartitioning
        transport.EnablePartitioning = true;
        #endregion
    }

    class MyEvent;
}

namespace Shipping
{
    interface IOrderAccepted : IEvent { }
    interface IOrderStatusChanged : IEvent { }

    class OrderAccepted : IOrderAccepted, IOrderStatusChanged { }
    class OrderDeclined : IOrderAccepted, IOrderStatusChanged { }

    class OrderAcceptedV2 : IOrderAccepted, IOrderStatusChanged { }
}