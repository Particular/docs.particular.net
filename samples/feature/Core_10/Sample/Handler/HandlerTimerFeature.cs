using NServiceBus.Features;

#region HandlerTimerFeature

public class HandlerTimerFeature :
    Feature
{
    public HandlerTimerFeature()
    {
        Enable<DiagnosticsFeature>();

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