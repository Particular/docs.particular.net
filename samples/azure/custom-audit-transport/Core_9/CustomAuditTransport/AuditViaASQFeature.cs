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
            //this is a send only endpoint so it does not have a queue, so its name can be anything
            var endpointConfiguration = new EndpointConfiguration("audit-via-asq");
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.SendOnly();

            var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true");
            transport.DelayedDelivery.DelayedDeliveryPoisonQueue = "audit-via-asq-delayed-poison";

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
