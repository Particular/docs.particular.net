namespace Core.Recoverability;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

#region custom-recoverability-action

public class EnableExternalBodyStorageBehavior : Behavior<IRecoverabilityContext>
{
    private readonly IExternalBodyStorage storage;

    public EnableExternalBodyStorageBehavior(IExternalBodyStorage storage)
    {
        this.storage = storage;
    }

    public async override Task Invoke(IRecoverabilityContext context, Func<Task> next)
    {
        if (context.RecoverabilityAction is MoveToError errorAction)
        {
            var message = context.FailedMessage;
            var bodyUrl = await storage.StoreBody(message.MessageId, message.Body);

            context.Metadata["body-url"] = bodyUrl;

            context.RecoverabilityAction = new SkipFailedMessageBody(errorAction.ErrorQueue);
        }

        await next();
    }

    class SkipFailedMessageBody : MoveToError
    {
        public SkipFailedMessageBody(string errorQueue) : base(errorQueue)
        {
        }

        public override IReadOnlyCollection<IRoutingContext> GetRoutingContexts(IRecoverabilityActionContext context)
        {
            var routingContexts = base.GetRoutingContexts(context);

            foreach (var routingContext in routingContexts)
            {
                // clear out the message body
                routingContext.Message.UpdateBody(ReadOnlyMemory<byte>.Empty);
            }

            return routingContexts;
        }
    }
}

#endregion

public interface IExternalBodyStorage
{
    Task<string> StoreBody(string messageId, ReadOnlyMemory<byte> body);
}