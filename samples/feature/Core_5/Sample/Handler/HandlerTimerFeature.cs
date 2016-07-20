using NServiceBus.Features;
using NServiceBus.Pipeline;

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
        pipeline.Register<Registration>();
    }

    class Registration :
        RegisterStep
    {
        public Registration()
            : base(
                stepId: "HandlerTimer",
                behavior: typeof(HandlerTimerBehavior),
                description: "Logs a warning if a handler take more than a specified time")
        {
            InsertBefore(WellKnownStep.InvokeHandlers);
        }
    }
}

#endregion