using NServiceBus.Features;
using NServiceBus.Pipeline;

#region SagaStateAuditFeature

public class SagaStateAuditFeature : Feature
{
    internal SagaStateAuditFeature()
    {
        EnableByDefault();
        DependsOn("Sagas");
        DependsOn<DiagnosticsFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register<Registration>();
    }

    class Registration : RegisterStep
    {
        public Registration()
            : base("SagaStateAudit", typeof(SagaStateAuditBehavior), "Logs Saga State")
        {
            InsertBefore(WellKnownStep.InvokeSaga);
        }
    }
}

#endregion