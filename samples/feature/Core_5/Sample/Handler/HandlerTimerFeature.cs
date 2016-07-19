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
        context.Pipeline.Register<Registration>();
    }

    class Registration :
        RegisterStep
    {
        public Registration()
            : base("HandlerTimer", typeof(HandlerTimerBehavior), "Logs a warning if a handler take more than a specified time")
        {
            InsertBefore(WellKnownStep.InvokeHandlers);
        }
    }
}

#endregion