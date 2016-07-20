using NServiceBus.Features;
using NServiceBus.Pipeline;

#region pipeline-config

public class StreamFeature :
    Feature
{
    internal StreamFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var pipeline = context.Pipeline;
        pipeline.Register<StreamReceiveRegistration>();
        pipeline.Register<StreamSendRegistration>();
    }
}

public class StreamReceiveRegistration :
    RegisterStep
{
    public StreamReceiveRegistration()
        : base(
            stepId: "StreamReceive",
            behavior: typeof(StreamReceiveBehavior),
            description: "Copies the shared data back to the logical messages")
    {
        InsertAfter("MutateIncomingMessages");
        // Note that in V6 invocation of handlers is in a different stage so no "before" is needed
    }
}

public class StreamSendRegistration :
    RegisterStep
{
    public StreamSendRegistration()
        : base(
            stepId: "StreamSend",
            behavior: typeof(StreamSendBehavior),
            description: "Saves the payload into the shared location")
    {
        InsertAfter("MutateOutgoingMessages");
        InsertBefore("ApplyTimeToBeReceived");
    }
}

#endregion