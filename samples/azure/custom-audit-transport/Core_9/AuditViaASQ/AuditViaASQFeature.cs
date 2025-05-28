using NServiceBus;
using NServiceBus.Features;
using static AuditTransportBehavior;

namespace CustomAuditTransport
{
    public class AuditViaASQFeature : Feature
    {
        #region featureSetup
        AuditViaASQFeature()
        {
            EnableByDefault();
            DependsOn<Audit>();

            Prerequisite(config =>
                config.Settings.TryGetAuditQueueAddress(out var auditQueueAddress) && !string.IsNullOrEmpty(auditQueueAddress),
                "No configured audit queue was found");
        }        

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.RegisterStartupTask(() => new AuditViaASQFeatureStartup());

            context.Pipeline.Register(new Registration());
        }
        #endregion
    }
}
