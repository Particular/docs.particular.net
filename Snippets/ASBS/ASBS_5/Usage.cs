using System;
using System.Security.Cryptography;
using System.Text;

using Azure.Identity;

using NServiceBus;
using Shipping;

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

        var topology = TopicTopology.Default;
        #region asb-interface-based-inheritance
        topology.SubscribeTo<IOrderStatusChanged>("Shipping.OrderAccepted");
        #endregion

        #region asb-interface-based-inheritance-declined
        topology.SubscribeTo<IOrderStatusChanged>("Shipping.OrderAccepted");
        topology.SubscribeTo<IOrderStatusChanged>("Shipping.OrderDeclined");
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
}