namespace Snippets4.Pipeline
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region SamplePipelineBehavior 4.5

    public class SampleBehavior : IBehavior<HandlerInvocationContext>
    {
        public void Invoke(HandlerInvocationContext context, Action next)
        {
            next();
        }
    }
    #endregion

}