using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class VerifyAdvertisedBehavior : Behavior<IOutgoingPublishContext>
{
    Type[] advertisedTypes;

    public VerifyAdvertisedBehavior(Type[] advertisedTypes)
    {
        this.advertisedTypes = advertisedTypes;
    }

    public override Task Invoke(IOutgoingPublishContext context, Func<Task> next)
    {
        if (advertisedTypes.Contains(context.Message.MessageType))
        {
            return next();
        }
        throw new Exception($"Message type {context.Message.MessageType} is not advertised. To use automatic routing you need to specify all published types.");
    }
}