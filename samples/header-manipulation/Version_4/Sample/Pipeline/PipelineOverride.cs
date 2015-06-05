using NServiceBus.MessageMutator;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#pragma warning disable 618
#region pipeline-config
public class PipelineOverride : IPipelineOverride
{
    public void Override(BehaviorList<ReceivePhysicalMessageContext> behaviorList)
    {
        behaviorList.InsertAfter<ApplyIncomingTransportMessageMutatorsBehavior, IncomingHeaderBehavior>();
    }
    public void Override(BehaviorList<SendPhysicalMessageContext> behaviorList)
    {
        behaviorList.InsertAfter<MutateOutgoingPhysicalMessageBehavior, OutgoingHeaderBehavior>();
    }
#endregion

    public void Override(BehaviorList<ReceiveLogicalMessageContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendLogicalMessageContext> behaviorList)
    {
    }
    public void Override(BehaviorList<HandlerInvocationContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendLogicalMessagesContext> behaviorList)
    {
    }

}