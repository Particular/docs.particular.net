using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
              Console.Title = "Sender";
              services.AddHostedService<InputLoopService>();
              services.AddSingleton<RotatingSessionKeyProvider>(); // Register the service

          }).UseNServiceBus(x =>
          {
              var endpointConfiguration = new EndpointConfiguration("c");

              endpointConfiguration.UsePersistence<LearningPersistence>();
              endpointConfiguration.UseSerialization<SystemJsonSerializer>();
              var routing = endpointConfiguration.UseTransport(new LearningTransport());

              routing.RouteToEndpoint(
                  typeof(SomeMessage),
                  "Samples.SessionFilter.Receiver"
              );

              #region add-filter-behavior

              var sessionKeyProvider = new RotatingSessionKeyProvider();
              var logger = new LoggerFactory().CreateLogger<FilterIncomingMessages>();

              endpointConfiguration.ApplySessionFilter(sessionKeyProvider, logger);
              #endregion

              return endpointConfiguration;
          });





}
