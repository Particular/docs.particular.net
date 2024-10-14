using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientUI;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("ClientUI");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var routing = endpointConfiguration.UseTransport(new LearningTransport());

        routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
        routing.RouteToEndpoint(typeof(CancelOrder), "Sales");

        builder.UseNServiceBus(endpointConfiguration);

        builder.Services.AddHostedService<InputLoopService>();

        await builder.Build().RunAsync();

    }
}
