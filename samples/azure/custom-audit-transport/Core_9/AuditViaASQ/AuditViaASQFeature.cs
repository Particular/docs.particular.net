using NServiceBus;
using NServiceBus.Features;
using static AuditTransportBehavior;

namespace CustomAuditTransport
{
    public class AuditViaASQFeature : Feature
    {
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
            //context.Pipeline.Register(
            //    stepId: "AuditTransport",
            //    behavior: typeof(AuditTransportBehavior),
            //    description: "sends audit messages to azure storage queues");
        }
    }
}
