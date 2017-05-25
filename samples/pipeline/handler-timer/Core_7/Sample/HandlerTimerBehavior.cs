﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region HandlerTimerBehavior
class HandlerTimerBehavior :
    Behavior<IInvokeHandlerContext>
{
    static ILog log = LogManager.GetLogger<HandlerTimerBehavior>();

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next()
                .ConfigureAwait(false);
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