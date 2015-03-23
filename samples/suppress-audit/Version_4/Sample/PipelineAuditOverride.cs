using NServiceBus.Audit;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Pipeline.MessageMutator;

#pragma warning disable 618
#region pipeline-config
public class PipelineAuditOverride : IPipelineOverride
{
    public void Override(BehaviorList<ReceivePhysicalMessageContext> behaviorList)
    {
        behaviorList.Replace<AuditBehavior, FilterAuditMessageTypeBehavior>();
    }

#endregion

    public void Override(BehaviorList<SendLogicalMessageContext> behaviorList)
    {
    }

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