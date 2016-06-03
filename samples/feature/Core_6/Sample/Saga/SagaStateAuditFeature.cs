using NServiceBus.Features;

#region SagaStateAuditFeature

public class SagaStateAuditFeature : Feature
{
    internal SagaStateAuditFeature()
    {
        EnableByDefault();
        DependsOn<Sagas>();
        DependsOn<DiagnosticsFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register("SagaStateAudit", typeof(SagaStateAuditBehavior), "Logs Saga State");
    }
}

#endregion