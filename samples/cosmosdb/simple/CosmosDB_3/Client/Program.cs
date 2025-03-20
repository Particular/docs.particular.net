using System;
using System.Threading.Tasks;
using Client;
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
             Console.Title = "Client";
             services.AddHostedService<InputLoopService>();

         }).UseNServiceBus(x =>
         {
             var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Simple.Client");
             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseTransport(new LearningTransport());
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             Console.WriteLine("Press 'S' to send a StartOrder message to the server endpoint");

             return endpointConfiguration;
         });
}