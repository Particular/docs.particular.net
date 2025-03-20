using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;

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
             services.AddHostedService<InputLoopService>();
         }).UseNServiceBus(x =>
         {
             Console.Title = "Sender";

             var endpointConfiguration = new EndpointConfiguration("Samples.Throttling.Sender");
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport());

             Console.WriteLine("Press any key");
             Console.ReadKey();

             return endpointConfiguration;
         });

}