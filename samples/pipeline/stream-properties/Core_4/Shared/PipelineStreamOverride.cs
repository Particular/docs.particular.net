using NServiceBus.MessageMutator;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Pipeline.MessageMutator;

#pragma warning disable 618
#region pipeline-config
public class PipelineStreamOverride :
    IPipelineOverride
{
    public void Override(BehaviorList<ReceiveLogicalMessageContext> behaviorList)
    {
        behaviorList.InsertAfter<ApplyIncomingMessageMutatorsBehavior, StreamReceiveBehavior>();
    }

    public void Override(BehaviorList<SendLogicalMessageContext> behaviorList)
    {
        behaviorList.InsertAfter<MutateOutgoingMessageBehavior, StreamSendBehavior>();
    }
#endregion

    public void Override(BehaviorList<HandlerInvocationContext> behaviorList)
    {
    }

    public void Override(BehaviorList<ReceivePhysicalMessageContext> behaviorList)
    {
    }
    public void Override(BehaviorList<SendLogicalMessagesContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendPhysicalMessageContext> behaviorList)
    {
    }
}