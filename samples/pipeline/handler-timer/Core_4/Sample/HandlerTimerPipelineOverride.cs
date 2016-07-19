using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast.Behaviors;

#pragma warning disable 618
#region pipeline-config
public class HandlerTimerPipelineOverride :
    IPipelineOverride
{
    public void Override(BehaviorList<HandlerInvocationContext> behaviorList)
    {
        behaviorList.InsertBefore<InvokeHandlersBehavior, HandlerTimerBehavior>();
    }

#endregion


    public void Override(BehaviorList<ReceiveLogicalMessageContext> behaviorList)
    {
    }

    public void Override(BehaviorList<ReceivePhysicalMessageContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendLogicalMessageContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendLogicalMessagesContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendPhysicalMessageContext> behaviorList)
    {
    }
}