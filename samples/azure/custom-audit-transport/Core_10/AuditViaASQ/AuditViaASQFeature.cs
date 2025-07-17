using NServiceBus.Features;

namespace AuditViaASQ;

public class AuditViaASQFeature : Feature
{
    #region featureSetup
    AuditViaASQFeature()
    {
        EnableByDefault();
        DependsOn<Audit>();
    }        

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.RegisterStartupTask(() => new AuditViaASQFeatureStartup());

        #region auditToDispatchConnectorReplacement
        context.Pipeline.Replace("AuditToDispatchConnector", new AuditDispatchTerminator());
        #endregion
    }
    #endregion
}
