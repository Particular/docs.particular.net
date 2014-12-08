using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region SamplePipelineBehavior

public class SampleBehavior : IBehavior<HandlerInvocationContext>
{
    public void Invoke(HandlerInvocationContext context, Action next)
    {
        next();
    }
}

#endregion
