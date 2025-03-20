using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport;
using NServiceBus.Transport.RabbitMQ;
using NServiceBus.Unicast.Messages;
using RabbitMQ.Client;

class MyRoutingTopology :
    IRoutingTopology
{
    public MyRoutingTopology(bool createDurableExchangesAndQueues)
    {
    }

    public ValueTask SetupSubscription(IChannel channel, MessageMetadata type, string subscriberName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask TeardownSubscription(IChannel channel, MessageMetadata type, string subscriberName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask Publish(IChannel channel, Type type, OutgoingMessage message, BasicProperties properties, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask Send(IChannel channel, string address, OutgoingMessage message, BasicProperties properties, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask RawSendInCaseOfFailure(IChannel channel, string address, ReadOnlyMemory<byte> body, BasicProperties properties, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask Initialize(IChannel channel, IEnumerable<string> receivingAddresses, IEnumerable<string> sendingAddresses, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask BindToDelayInfrastructure(IChannel channel, string address, string deliveryExchange, string routingKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}