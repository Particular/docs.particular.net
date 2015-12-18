// ReSharper disable UnusedParameter.Local
namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    public class AbortingPipeline
    {
        #region AbortPipelineWithBehavior
        public class Behavior : Behavior<IIncomingLogicalMessageContext>
        {
            public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
            {
                if (ShouldPipelineContinue(context))
                {
                    await next();
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

        #region AbortHandler
        class Handler : IHandleMessages<MyMessage>
        {
            public Task Handle(MyMessage message, IMessageHandlerContext context)
            {
                context.DoNotContinueDispatchingCurrentMessageToHandlers();
                return Task.FromResult(0);
            }
        }
        #endregion

        class MyMessage
        {
        }
    }
}