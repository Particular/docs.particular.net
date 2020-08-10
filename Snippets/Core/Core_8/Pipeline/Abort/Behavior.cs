// ReSharper disable UnusedParameter.Local
namespace Core8.Pipeline.Abort
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region AbortPipelineWithBehavior

    public class Behavior :
        Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if (ShouldPipelineContinue(context))
            {
                return next();
            }
            // since next is not invoke all downstream behaviors will be skipped
            return Task.CompletedTask;
        }

        bool ShouldPipelineContinue(IIncomingLogicalMessageContext context)
        {
            // the custom logic to determine if the pipeline should continue
            return true;
        }
    }

    #endregion

}