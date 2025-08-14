namespace Core.Audit;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Audit;
using NServiceBus.Pipeline;

#region custom-audit-action

public class EnableExternalBodyStorageBehavior : Behavior<IAuditContext>
{
    private readonly IExternalBodyStorage storage;

    public EnableExternalBodyStorageBehavior(IExternalBodyStorage storage)
    {
        this.storage = storage;
    }

    public async override Task Invoke(IAuditContext context, Func<Task> next)
    {
        var message = context.Message;
        var bodyUrl = await storage.StoreBody(message.MessageId, message.Body);

        context.AuditMetadata["body-url"] = bodyUrl;

        context.AuditAction = new SkipAuditMessageBody();

        await next();
    }

    class SkipAuditMessageBody : RouteToAudit
    {
        public override IReadOnlyCollection<IRoutingContext> GetRoutingContexts(IAuditActionContext context)
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