using NServiceBus.Features;

#region HandlerTimerFeature

public class HandlerTimerFeature : Feature
{
    internal HandlerTimerFeature()
    {
        EnableByDefault();
        DependsOn<DiagnosticsFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register("HandlerTimer", typeof(HandlerTimerBehavior), "Logs handler time");
    }
}

#endregion