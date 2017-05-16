using System;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NServiceBus;
using Owin;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.TransientClients.ClientHub";

        var endpointConfiguration = new EndpointConfiguration("Samples.TransientClients.ClientHub");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();

        routing.RegisterPublisher(
            eventType: typeof(StockTick),
            publisherEndpoint: "Samples.TransientClients.Publisher");

        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.Conventions().DefiningEventsAs(t => t == typeof(StockTick));

        endpointConfiguration.EnableInstallers();

        var url = "http://localhost:8080";

        using (WebApp.Start<OwinStartup>(url))
        {
            Console.WriteLine($"SignalR server running at {url}");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("NServiceBus subscriber running");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);

            await endpointInstance.Unsubscribe<StockTick>();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
    class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
