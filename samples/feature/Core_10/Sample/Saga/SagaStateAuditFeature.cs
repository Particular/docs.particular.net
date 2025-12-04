using NServiceBus.Features;

#region SagaStateAuditFeature

public class SagaStateAuditFeature :
    Feature
{
    public SagaStateAuditFeature()
    {
        DependsOn<Sagas>();
        DependsOn<DiagnosticsFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var pipeline = context.Pipeline;
        pipeline.Register(
            stepId: "SagaStateAudit",
            behavior: typeof(SagaStateAuditBehavior),
            description: "Logs Saga State");
    }
}

#endregion