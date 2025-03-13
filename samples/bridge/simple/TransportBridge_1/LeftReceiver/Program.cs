using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

static class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             Console.Title = "LeftReceiver";
         }).UseNServiceBus(x =>
         {
             var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftReceiver");
             endpointConfiguration.UsePersistence<LearningPersistence>();
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport());

             endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
             endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

             endpointConfiguration.SendFailedMessagesTo("error");
             endpointConfiguration.EnableInstallers();

             return endpointConfiguration;
         });
}
