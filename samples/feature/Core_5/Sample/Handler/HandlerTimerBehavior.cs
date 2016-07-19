using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region HandlerTimerBehavior

class HandlerTimerBehavior :
    IBehavior<IncomingContext>
{
    CustomLogger logger;

    public HandlerTimerBehavior(CustomLogger logger)
    {
        this.logger = logger;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        var handlerName = context.MessageHandler.Instance.GetType().Name;
        using (logger.StartTimer(handlerName))
        {
            next();
        }
    }
}

#endregion
