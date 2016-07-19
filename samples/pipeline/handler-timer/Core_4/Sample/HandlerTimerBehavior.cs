using System;
using System.Diagnostics;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
#pragma warning disable 618

#region HandlerTimerBehavior
class HandlerTimerBehavior :
    IBehavior<HandlerInvocationContext>
{
    static ILog log = LogManager.GetLogger(typeof(HandlerTimerBehavior));

    public void Invoke(HandlerInvocationContext context, Action next)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            next();
        }
        finally
        {
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            if (elapsedMilliseconds > 500)
            {
                var handlerName = context.MessageHandler.Instance.GetType().Name;
                log.Warn($"{handlerName} took {elapsedMilliseconds}ms to process");
            }
        }
    }
}
#endregion