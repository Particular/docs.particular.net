// ReSharper disable UnusedParameter.Local
namespace Core5.Pipeline.Abort
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region AbortPipelineWithBehavior
    public class Behavior :
        IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            if (ShouldPipelineContinue(context))
            {
                next();
            }
            // since next is not invoke all downstream behaviors will be skipped
        }

        bool ShouldPipelineContinue(IncomingContext context)
        {
            // the custom logic to determine if the pipeline should continue
            return true;
        }
    }
    #endregion
}