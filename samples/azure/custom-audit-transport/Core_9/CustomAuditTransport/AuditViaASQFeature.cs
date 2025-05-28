using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //Prerequisite(config => config.Settings.GetOrDefault<AuditConfigReader.Result>() != null, "No configured audit queue was found");
            //Prerequisite(() => !EndpointConfigurationExtensions.AuditProcessedMessagesTo(EndpointConfigurationExtensions.GetCurrentEndpointConfiguration(), "audit").IsNullOrEmpty(), "Audit is not configured.");
        }

        protected override void Setup(FeatureConfigurationContext context)
        {                        
            //TOO get queue name from context
            var endpointConfiguration = new EndpointConfiguration("audit");
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
