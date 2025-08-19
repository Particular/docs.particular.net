namespace Core.Pipeline;

using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region SetContextBetweenIncomingAndOutgoing

public class SetContextBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        // set the state
        context.Extensions.Set(new SharedData());

        await next();
    }
}

public class GetContextBehavior :
    Behavior<IOutgoingPhysicalMessageContext>
{
    public override async Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {

        if (context.Extensions.TryGet<SharedData>(out var state))
        {
            // work with the state
        }

        await next();
    }
}

#endregion