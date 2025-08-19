namespace Core.Pipeline;

using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region NoActionPipelineBehavior
public class NoActionBehavior :
    Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        //no action taken. empty behavior
        await next();
    }
}
#endregion