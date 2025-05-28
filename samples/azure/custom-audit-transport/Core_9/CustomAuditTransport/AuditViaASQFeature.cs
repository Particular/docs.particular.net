using System;
using NServiceBus;
using NServiceBus.Features;

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
            if (!context.Settings.TryGetAuditQueueAddress(out var auditQueueAddress))
            {
                throw new InvalidOperationException("No configured audit queue was found");
            }

            var endpointConfiguration = new EndpointConfiguration(auditQueueAddress);
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.SendOnly();

            var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true");
            transport.DelayedDelivery.DelayedDeliveryPoisonQueue = "audit-delayed-poison";

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseTransport(transport);

            context.RegisterStartupTask(() => new AuditViaASQFeatureStartup(endpointConfiguration));

            var pipeline = endpointConfiguration.Pipeline;
            context.Pipeline.Register(
                stepId: "AuditTransport",
                behavior: typeof(AuditTransportBehavior),
                description: "sends audit messages to azure storage queues");
        }
    }
}
