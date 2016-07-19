using NServiceBus.Features;
using NServiceBus.Pipeline;

#region pipeline-config
public class HeaderFeature :
    Feature
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

public class IncomingHeaderRegistration :
    RegisterStep
{
    public IncomingHeaderRegistration()
        : base(
            stepId: "IncomingHeaderManipulation",
            behavior: typeof(IncomingHeaderBehavior),
            description: "Manipulates incoming headers")
    {
        InsertAfter(WellKnownStep.MutateIncomingTransportMessage);
        InsertBefore(WellKnownStep.InvokeHandlers);
    }
}

public class OutgoingHeaderRegistration :
    RegisterStep
{
    public OutgoingHeaderRegistration()
        : base(
            stepId: "OutgoingHeaderManipulation",
            behavior: typeof(OutgoingHeaderBehavior),
            description: "Manipulates outgoing headers")
    {
        InsertAfter(WellKnownStep.MutateOutgoingTransportMessage);
        InsertBefore(WellKnownStep.DispatchMessageToTransport);
    }
}

#endregion