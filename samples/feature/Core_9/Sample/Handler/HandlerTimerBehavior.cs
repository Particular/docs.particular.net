using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region HandlerTimerBehavior
class HandlerTimerBehavior(CustomLogger logger) :
    Behavior<IInvokeHandlerContext>
{
    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        var handlerName = context.MessageHandler.Instance.GetType().Name;
        using (logger.StartTimer(handlerName))
        {
            await next();
        }
    }
}
#endregion