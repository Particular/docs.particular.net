#pragma warning disable 618
namespace Core4.Pipeline
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region SamplePipelineBehavior 4.5

    public class SampleBehavior :
        IBehavior<HandlerInvocationContext>
    {
        public void Invoke(HandlerInvocationContext context, Action next)
        {
            // custom logic before calling the next step in the pipeline.

            next();

            // custom logic after all inner steps in the pipeline completed.
        }
    }
    #endregion

}