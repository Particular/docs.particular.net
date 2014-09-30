namespace Snippets_5.Pipeline
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region SamplePipelineBehavior
    public class SampleBehavior : IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            next();
        }
    }
    #endregion
}
