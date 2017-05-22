using NServiceBus.Features;

#region HandlerTimerFeature

public class HandlerTimerFeature :
    Feature
{
    internal HandlerTimerFeature()
    {
        EnableByDefault();
        DependsOn<DiagnosticsFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var pipeline = context.Pipeline;
        pipeline.Register(
            stepId: "HandlerTimer",
            behavior: typeof(HandlerTimerBehavior),
            description: "Logs handler time");
    }
}

#endregion