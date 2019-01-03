using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Raw;
using NServiceBus.Router;
using NServiceBus.Transport;

class RandomDuplicator : ChainTerminator<PostroutingContext>
{
    IRawEndpoint dispatcher;
    Random random = new Random();

    public RandomDuplicator(IRawEndpoint dispatcher)
    {
        this.dispatcher = dispatcher;
    }

    protected override async Task<bool> Terminate(PostroutingContext context)
    {
        if (random.Next(2) == 0)
        {
            await dispatcher.Dispatch(new TransportOperations(Copy(context.Messages)), context.Get<TransportTransaction>(), context)
                .ConfigureAwait(false);
            return true;
        }
        return false;
    }

    static TransportOperation[] Copy(TransportOperation[] operations)
    {
        return operations
            .Select(o => new TransportOperation(Copy(o.Message), o.AddressTag, o.RequiredDispatchConsistency, o.DeliveryConstraints))
            .ToArray();
    }

    static OutgoingMessage Copy(OutgoingMessage message)
    {
        return new OutgoingMessage(message.MessageId, new Dictionary<string, string>(message.Headers), message.Body);
    }
}