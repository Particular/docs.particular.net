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