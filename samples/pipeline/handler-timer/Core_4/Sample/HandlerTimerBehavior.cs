using System;
using System.Diagnostics;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
#pragma warning disable 618

#region HandlerTimerBehavior
class HandlerTimerBehavior : IBehavior<HandlerInvocationContext>
{
    static ILog logger = LogManager.GetLogger(typeof(HandlerTimerBehavior));

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
                logger.WarnFormat("{1} took {0}ms to process", elapsedMilliseconds, handlerName);
            }
        }
    }
}
#endregion