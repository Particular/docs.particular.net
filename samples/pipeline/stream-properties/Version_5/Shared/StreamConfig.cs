using NServiceBus.Features;
using NServiceBus.Pipeline;

#region pipeline-config

public class StreamFeature : Feature
{
    internal StreamFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register<StreamReceiveRegistration>();
        context.Pipeline.Register<StreamSendRegistration>();
    }
}

public class StreamReceiveRegistration : RegisterStep
{
    public StreamReceiveRegistration()
        : base("StreamReceive", typeof(StreamReceiveBehavior), "Copies the shared data back to the logical messages")
    {
        InsertAfter(WellKnownStep.MutateIncomingMessages);
        InsertBefore(WellKnownStep.InvokeHandlers);
    }
}

public class StreamSendRegistration : RegisterStep
{
    public StreamSendRegistration()
        : base("StreamSend", typeof(StreamSendBehavior), "Saves the payload into the shared location")
    {
        InsertAfter(WellKnownStep.MutateOutgoingMessages);
        InsertBefore(WellKnownStep.CreatePhysicalMessage);
    }
}
#endregion