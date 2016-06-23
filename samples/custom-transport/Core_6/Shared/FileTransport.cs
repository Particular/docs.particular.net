
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Performance.TimeToBeReceived;
using NServiceBus.Routing;
using NServiceBus.Settings;
using NServiceBus.Transports;

#region TransportDefinition
public class FileTransport : TransportDefinition
{
    public override bool RequiresConnectionString => false;

    protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        return new FileTransportInfrastructure();
    }

    public override string ExampleConnectionStringForErrorMessage { get; } = "";
}

public class FileTransportInfrastructure : TransportInfrastructure
{
    public override TransportReceiveInfrastructure ConfigureReceiveInfrastructure()
    {
        return new TransportReceiveInfrastructure(() => new FileTransportMessagePump(), () => new FileTransportQueueCreator(), () => Task.FromResult(StartupCheckResult.Success));
    }

    public override TransportSendInfrastructure ConfigureSendInfrastructure()
    {
        return new TransportSendInfrastructure(() => new Dispatcher(), () => Task.FromResult(StartupCheckResult.Success));
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
        return Path.Combine(logicalAddress.EndpointInstance.Endpoint,
            logicalAddress.EndpointInstance.Discriminator ?? "",
            logicalAddress.Qualifier ?? "");
    }

    public override IEnumerable<Type> DeliveryConstraints
    {
        get { yield return typeof(DiscardIfNotReceivedBefore);}
    }

    public override TransportTransactionMode TransactionMode => TransportTransactionMode.SendsAtomicWithReceive;

    public override OutboundRoutingPolicy OutboundRoutingPolicy => new OutboundRoutingPolicy(OutboundRoutingType.Unicast, OutboundRoutingType.Unicast, OutboundRoutingType.Unicast);
}
#endregion