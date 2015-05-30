using NServiceBus.Features;
using NServiceBus.Pipeline;

#region pipeline-config
public class HeaderFeature : Feature
{
    internal HeaderFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register<IncomingHeaderRegistration>();
        context.Pipeline.Register<OutgoingHeaderRegistration>();
    }
}
public class IncomingHeaderRegistration : RegisterStep
{
    public IncomingHeaderRegistration()
        : base("IncomingHeaderManipulation", typeof(IncomingHeaderBehavior), "Copies the shared data back to the logical messages")
    {
        InsertAfter(WellKnownStep.MutateIncomingTransportMessage);
        InsertBefore(WellKnownStep.InvokeHandlers);
    }
}

public class OutgoingHeaderRegistration : RegisterStep
{
    public OutgoingHeaderRegistration()
        : base("OutgoingHeaderManipulation", typeof(OutgoingHeaderBehavior), "Saves the payload into the shared location")
    {
        InsertAfter(WellKnownStep.MutateOutgoingTransportMessage);
        InsertBefore(WellKnownStep.DispatchMessageToTransport);
    }
}
#endregion