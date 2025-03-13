using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
         {
             var endpointConfiguration = new EndpointConfiguration(
            "Samples.Bridge.Endpoint");

             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport
             {
                 StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
             });

             var recoverability = endpointConfiguration.Recoverability();
             recoverability.Immediate(
                 customizations: immediate =>
                 {
                     immediate.NumberOfRetries(0);
                 });
             recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

             endpointConfiguration.SendFailedMessagesTo("error");
             endpointConfiguration.AuditProcessedMessagesTo("audit");
             endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
             endpointConfiguration.EnableInstallers();
             endpointConfiguration.EnableMetrics().SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

             return endpointConfiguration;
         }).ConfigureServices((hostContext, services) =>
         {
             Console.Title = "Endpoint";
             services.AddHostedService<InputLoopService>();

         });

}
