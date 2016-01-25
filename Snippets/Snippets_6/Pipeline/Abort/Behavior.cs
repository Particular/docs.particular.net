// ReSharper disable UnusedParameter.Local
namespace Snippets6.Pipeline.Abort
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region AbortPipelineWithBehavior

    public class Behavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if (ShouldPipelineContinue(context))
            {
                await next().ConfigureAwait(false);
            }
            // since next is not invoke all downstream behaviors will be skipped
        }

        bool ShouldPipelineContinue(IIncomingLogicalMessageContext context)
        {
            // your custom logic to determine if the pipeline should continue 
            return true;
        }
    }

    #endregion

}