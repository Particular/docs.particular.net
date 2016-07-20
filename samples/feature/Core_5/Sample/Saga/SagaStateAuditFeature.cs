using NServiceBus.Features;
using NServiceBus.Pipeline;

#region SagaStateAuditFeature

public class SagaStateAuditFeature :
    Feature
{
    internal SagaStateAuditFeature()
    {
        EnableByDefault();
        DependsOn<Sagas>();
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
                stepId: "SagaStateAudit",
                behavior: typeof(SagaStateAuditBehavior),
                description: "Logs Saga State")
        {
            InsertBefore(WellKnownStep.InvokeSaga);
        }
    }
}

#endregion