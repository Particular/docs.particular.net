using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Billing
{    
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Billing";
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseNServiceBus(context =>
                       {
                           var endpointConfiguration = new EndpointConfiguration("Billing");

                           endpointConfiguration.UseTransport<LearningTransport>();

                           endpointConfiguration.SendFailedMessagesTo("error");
                           endpointConfiguration.AuditProcessedMessagesTo("audit");
                           endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

                           var metrics = endpointConfiguration.EnableMetrics();
                           metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

                           return endpointConfiguration;
                       });
        }
    }
}
