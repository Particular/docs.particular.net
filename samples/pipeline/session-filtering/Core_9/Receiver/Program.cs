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
             services.AddHostedService<ReceivingLoopService>();
             // Register FilterIncomingMessages if it's a service
             services.AddSingleton<FilterIncomingMessages>();



         }).UseNServiceBus(x =>
         {
             Console.Title = "Receiver";
             var endpointConfiguration = new EndpointConfiguration("Samples.SessionFilter.Receiver");

             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport());

             var sessionKeyProvider = new RotatingSessionKeyProvider();
             // Register FilterIncomingMessages if it's a service
             var logger = new LoggerFactory().CreateLogger<FilterIncomingMessages>();
             endpointConfiguration.ApplySessionFilter(sessionKeyProvider, logger);


             return endpointConfiguration;
         });




}

