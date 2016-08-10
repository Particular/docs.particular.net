
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Performance.TimeToBeReceived;
using NServiceBus.Routing;
using NServiceBus.Settings;
using NServiceBus.Transport;

#region TransportDefinition
public class FileTransport :
    TransportDefinition
{
    public override bool RequiresConnectionString => false;

    public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        return new FileTransportInfrastructure();
    }

    public override string ExampleConnectionStringForErrorMessage { get; } = "";
}

public class FileTransportInfrastructure :
    TransportInfrastructure
{
    public override TransportReceiveInfrastructure ConfigureReceiveInfrastructure()
    {
        return new TransportReceiveInfrastructure(
            messagePumpFactory: () => new FileTransportMessagePump(),
            queueCreatorFactory: () => new FileTransportQueueCreator(),
            preStartupCheck: () => Task.FromResult(StartupCheckResult.Success));
    }

    public override TransportSendInfrastructure ConfigureSendInfrastructure()
    {
        return new TransportSendInfrastructure(
            dispatcherFactory: () => new Dispatcher(),
            preStartupCheck: () => Task.FromResult(StartupCheckResult.Success));
    }

    public override TransportSubscriptionInfrastructure ConfigureSubscriptionInfrastructure()
    {
        throw new NotImplementedException();
    }

    public override EndpointInstance BindToLocalEndpoint(EndpointInstance instance)
    {
        return instance;
    }

    public override string ToTransportAddress(LogicalAddress logicalAddress)
    {
        var endpointInstance = logicalAddress.EndpointInstance;
        var discriminator = endpointInstance.Discriminator ?? "";
        var qualifier = logicalAddress.Qualifier ?? "";
        return Path.Combine(endpointInstance.Endpoint, discriminator, qualifier);
    }

    public override IEnumerable<Type> DeliveryConstraints
    {
        get
        {
            yield return typeof(DiscardIfNotReceivedBefore);
        }
    }

    public override TransportTransactionMode TransactionMode
    {
        get
        {
            return TransportTransactionMode.SendsAtomicWithReceive;
        }
    }

    public override OutboundRoutingPolicy OutboundRoutingPolicy
    {
        get
        {
            return new OutboundRoutingPolicy(
                sends: OutboundRoutingType.Unicast,
                publishes: OutboundRoutingType.Unicast,
                replies: OutboundRoutingType.Unicast);
        }
    }
}

#endregion