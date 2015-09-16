using System;
using System.Diagnostics;
using NServiceBus.Logging;
using NServiceBus.Pipeline.Contexts;

#region HandlerTimerBehavior
class HandlerTimerBehavior : HandlingStageBehavior
{
    static ILog log = LogManager.GetLogger(typeof(HandlerTimerBehavior));

    public override void Invoke(Context context, Action next)
    {
        var stopwatch = Stopwatch.StartNew();
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
                log.WarnFormat("{1} took {0}ms to process", elapsedMilliseconds, handlerName);
            }
        }
        next();
    }
}
#endregion