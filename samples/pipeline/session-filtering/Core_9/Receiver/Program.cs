using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Receiver;

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
             Console.Title = "Server";
             services.AddSingleton<RotatingSessionKeyProvider>(); // Register the service
             services.AddHostedService<RecevingLoopService>();
             services.AddSingleton<ILogger<FilterIncomingMessages>>();


         }).UseNServiceBus(static x =>
         {
             Console.Title = "Receiver";
             var endpointConfiguration = new EndpointConfiguration("Samples.SessionFilter.Receiver");

             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport());

             var sessionKeyProvider = new RotatingSessionKeyProvider();
             endpointConfiguration.ApplySessionFilter(sessionKeyProvider);


             return endpointConfiguration;
         });




}

