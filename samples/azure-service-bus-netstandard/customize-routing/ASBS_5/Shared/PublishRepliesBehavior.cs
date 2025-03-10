using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

public class PublishRepliesBehavior : Behavior<IOutgoingReplyContext>
{
    public override Task Invoke(IOutgoingReplyContext context, Func<Task> next)
    {
        context.Extensions.Set(new PublishAllMessagesBehavior.State());
        return next();
    }
}