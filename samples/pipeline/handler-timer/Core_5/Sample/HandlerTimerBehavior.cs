using System;
using System.Diagnostics;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region HandlerTimerBehavior
class HandlerTimerBehavior : IBehavior<IncomingContext>
{
    static ILog log = LogManager.GetLogger<HandlerTimerBehavior>();

    public void Invoke(IncomingContext context, Action next)
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
