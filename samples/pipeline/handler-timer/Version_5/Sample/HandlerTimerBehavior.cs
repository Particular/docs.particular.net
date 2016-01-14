using System;
using System.Diagnostics;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region HandlerTimerBehavior
class HandlerTimerBehavior : IBehavior<IncomingContext>
{
    static ILog logger = LogManager.GetLogger<HandlerTimerBehavior>();

    public void Invoke(IncomingContext context, Action next)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            next();
        }
        finally
        {
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            if (elapsedMilliseconds > 500)
            {
                string handlerName = context.MessageHandler.Instance.GetType().Name;
                logger.WarnFormat("{1} took {0}ms to process", elapsedMilliseconds, handlerName);
            }
        }
    }
}
#endregion
