using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Billing
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Billing";

            var endpointConfiguration = new EndpointConfiguration("Billing");

            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

            var metrics = endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));


            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}