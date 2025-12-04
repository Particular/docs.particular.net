using NServiceBus.Features;

namespace AuditViaASQ;

public class AuditViaASQFeature : Feature
{
    #region featureSetup

    public AuditViaASQFeature()
    {
        DependsOn<Audit>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.RegisterStartupTask(new AuditViaASQFeatureStartup());

        #region auditToDispatchConnectorReplacement
        context.Pipeline.Replace("AuditToDispatchConnector", new AuditDispatchTerminator());
        #endregion
    }
    #endregion
}
