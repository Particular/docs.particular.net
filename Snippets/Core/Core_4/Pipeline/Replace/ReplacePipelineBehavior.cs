#pragma warning disable 618
namespace Core4.Pipeline.Replace
{
    using System;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;
    using NServiceBus.Unicast.Behaviors;

    #region ReplacePipelineStep 4.5
    class ReplaceExistingBehavior :
        PipelineOverride
    {
        public override void Override(BehaviorList<HandlerInvocationContext> behaviorList)
        {
            behaviorList.Replace<InvokeHandlersBehavior, MyInvokeHandlersBehavior>();
        }

        // Classes inheriting from PipelineOverride are registered by convention.
        // No need to explicitly register.
    }
    #endregion

    public class MyInvokeHandlersBehavior :
        IBehavior<HandlerInvocationContext>
    {
        public void Invoke(HandlerInvocationContext context, Action next)
        {
        }
    }
}