#region ShippingProgram

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Shipping
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Shipping";
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseNServiceBus(context =>
                       {
                           // Define the endpoint name
                           var endpointConfiguration = new EndpointConfiguration("Shipping");

                           // Select the learning (filesystem-based) transport to communicate
                           // with other endpoints
                           endpointConfiguration.UseTransport<LearningTransport>();

                           // Enable monitoring errors, auditing, and heartbeats with the
                           // Particular Service Platform tools
                           endpointConfiguration.SendFailedMessagesTo("error");
                           endpointConfiguration.AuditProcessedMessagesTo("audit");
                           endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

                           // Enable monitoring endpoint performance
                           var metrics = endpointConfiguration.EnableMetrics();
                           metrics.SendMetricDataToServiceControl("Particular.Monitoring", 
                               TimeSpan.FromMilliseconds(500));

                           // Return the completed configuration
                           return endpointConfiguration;
                       });
        }
    }
}

#endregion
