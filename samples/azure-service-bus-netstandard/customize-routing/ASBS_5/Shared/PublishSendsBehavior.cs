using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

public class PublishSendsBehavior : Behavior<IOutgoingSendContext>
{
    public override Task Invoke(IOutgoingSendContext context, Func<Task> next)
    {
        context.Extensions.Set(new PublishAllMessagesBehavior.State());
        return next();
    }
}