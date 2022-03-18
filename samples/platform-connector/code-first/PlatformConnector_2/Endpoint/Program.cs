using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

public class Program
{
    public static async Task Main()
    {
        Console.Title = "Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Endpoint");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();

        #region createConnectionDetails

        var platformConnection = new ServicePlatformConnectionConfiguration
        {
            ErrorQueue = "error",
            Heartbeats = new ServicePlatformHeartbeatConfiguration
            {
                Enabled = true,
                HeartbeatsQueue = "Particular.ServiceControl"
            },
            CustomChecks = new ServicePlatformCustomChecksConfiguration
            {
                Enabled = true,
                CustomChecksQueue = "Particular.ServiceControl"
            },
            MessageAudit = new ServicePlatformMessageAuditConfiguration
            {
                Enabled = true,
                AuditQueue = "audit"
            },
            SagaAudit = new ServicePlatformSagaAuditConfiguration
            {
                Enabled = true,
                SagaAuditQueue = "audit"
            },
            Metrics = new ServicePlatformMetricsConfiguration
            {
                Enabled = true,
                MetricsQueue = "Particular.Monitoring",
                Interval = TimeSpan.FromSeconds(1),
                InstanceId = "uniqueInstanceId"
            }
        };
        #endregion

        #region configureConnection
        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration);

        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            await endpoint.SendLocal(new BusinessMessage { BusinessId = Guid.NewGuid() });
        }

        await endpoint.Stop();
    }
}