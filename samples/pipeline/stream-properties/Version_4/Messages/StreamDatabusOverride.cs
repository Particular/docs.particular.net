using NServiceBus.DataBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#pragma warning disable 618
public class StreamDatabusOverride : IPipelineOverride
{
    public void Override(BehaviorList<HandlerInvocationContext> behaviorList)
    {
    }

    public void Override(BehaviorList<ReceiveLogicalMessageContext> behaviorList)
    {
        behaviorList.InsertBefore<DataBusReceiveBehavior, StreamReceiveBehavior>();
    }

    public void Override(BehaviorList<ReceivePhysicalMessageContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendLogicalMessageContext> behaviorList)
    {
        behaviorList.InsertBefore<DataBusSendBehavior, StreamSendBehavior>();
    }

    public void Override(BehaviorList<SendLogicalMessagesContext> behaviorList)
    {
    }

    public void Override(BehaviorList<SendPhysicalMessageContext> behaviorList)
    {
    }
}