using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport;

class Forwarder
{
    QueueAddress logicalAddress;
    HashSet<string> knownPartitionKeys;
    ITransportAddressResolver transportAddressResolver;

    public Forwarder(HashSet<string> knownPartitionKeys, ITransportAddressResolver transportAddressResolver, QueueAddress logicalAddress)
    {
        this.knownPartitionKeys = knownPartitionKeys;
        this.transportAddressResolver = transportAddressResolver;
        this.logicalAddress = logicalAddress;
    }

    public Task Forward(IMessageProcessingContext context, string messagePartitionKey)
    {
        if (!knownPartitionKeys.Contains(messagePartitionKey))
        {
            throw new PartitionMappingFailedException($"User mapped key {messagePartitionKey} does not match any known partition key values");
        }

        var individualizedAddress = new QueueAddress(logicalAddress.BaseAddress, messagePartitionKey, null, null);
        var destination = transportAddressResolver.ToTransportAddress(individualizedAddress);
        return context.ForwardCurrentMessageTo(destination);
    }
}