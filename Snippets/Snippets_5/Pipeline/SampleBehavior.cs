namespace Snippets5.Pipeline
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region SamplePipelineBehavior

    public class SampleBehavior : IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            // custom logic before calling the next step in the pipeline.

            next();

            // custom logic after all inner steps in the pipeline completed.
        }
    }

    #endregion
}