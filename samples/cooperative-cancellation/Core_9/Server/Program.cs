using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Server;

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
             Console.Title = "Cancellation";
             services.AddHostedService<InputLoopService>();
         }).UseNServiceBus(x =>
         {
             var endpointConfiguration = new EndpointConfiguration("Samples.Cooperative.Cancellation");
             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport());

             return endpointConfiguration;
         });
}
