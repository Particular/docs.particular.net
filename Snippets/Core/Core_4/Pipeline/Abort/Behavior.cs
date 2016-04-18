// ReSharper disable UnusedParameter.Local
namespace Core4.Pipeline.Abort
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region AbortPipelineWithBehavior
    public class Behavior : IBehavior<HandlerInvocationContext>
    {
        public void Invoke(HandlerInvocationContext context, Action next)
        {
            if (ShouldPipelineContinue(context))
            {
                next();
            }
            // since next is not invoke all downstream behaviors will be skipped
        }

        bool ShouldPipelineContinue(HandlerInvocationContext context)
        {
            // your custom logic to determine if the pipeline should continue 
            return true;
        }
    }
    #endregion
}