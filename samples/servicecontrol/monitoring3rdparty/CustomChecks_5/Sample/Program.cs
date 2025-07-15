using System;
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
         .ConfigureServices((hostContext, services) =>
         {
         }).UseNServiceBus(x =>
         {
             Console.Title = "Monitor3rdParty";
             var endpointConfiguration = new EndpointConfiguration("Samples.CustomChecks.Monitor3rdParty");
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport<LearningTransport>();

             endpointConfiguration.ReportCustomChecksTo("Particular.ServiceControl");
             
             return endpointConfiguration;
         });

}