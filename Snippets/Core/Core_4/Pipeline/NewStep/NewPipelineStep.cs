#pragma warning disable 618
namespace Core4.Pipeline.NewStep
{
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;
    using NServiceBus.Unicast.Behaviors;

    #region NewPipelineStep 4.5

    class NewStepInPipeline :
        PipelineOverride
    {
        public override void Override(BehaviorList<HandlerInvocationContext> behaviorList)
        {
            behaviorList.InsertAfter<InvokeHandlersBehavior, SampleBehavior>();
        }

        // Classes inheriting from PipelineOverride are registered by convention.
        // No need to explicitly register.
    }

    #endregion
}