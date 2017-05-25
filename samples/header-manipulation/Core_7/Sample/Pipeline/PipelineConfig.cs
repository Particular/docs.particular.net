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
        var pipeline = context.Pipeline;
        pipeline.Register<IncomingHeaderRegistration>();
        pipeline.Register<OutgoingHeaderRegistration>();
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
    }
}

#endregion