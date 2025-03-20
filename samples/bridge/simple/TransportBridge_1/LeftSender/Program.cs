using System;
using System.Threading.Tasks;
using LeftSender;
using Microsoft.Extensions.DependencyInjection;
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
         .UseNServiceBus(x =>
         {

             var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftSender");
             endpointConfiguration.UsePersistence<LearningPersistence>();

             endpointConfiguration.Conventions().DefiningCommandsAs(t => t.Name == "PlaceOrder");
             endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
             endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             var routing = endpointConfiguration.UseTransport(new LearningTransport());
             routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.Bridge.RightReceiver");

             endpointConfiguration.SendFailedMessagesTo("error");
             endpointConfiguration.EnableInstallers();

             return endpointConfiguration;
         }).ConfigureServices((hostContext, services) =>
         {
             services.AddHostedService<InputLoopService>();
             Console.Title = "LeftSender";
         });



}
