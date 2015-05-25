// ReSharper disable UnusedParameter.Local
namespace AbortPipeline
{
    using System;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

    #region AbortHandler

    class AbortHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public AbortHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            // you may also want to log a reason here
            bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }

    #endregion
    #region AbortPipelineWithBehavior
    public class SampleBehavior : IBehavior<HandlerInvocationContext>
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
    public class MyMessage
    {

    }
}