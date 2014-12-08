using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast.Behaviors;

#region NewStepInPipeline
class NewStepInPipeline : PipelineOverride
{
    public override void Override(BehaviorList<HandlerInvocationContext> behaviorList)
    {
        behaviorList.InsertAfter<InvokeHandlersBehavior, SampleBehavior>();
    }
}
#endregion

