using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.Receiver");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.EnableCallbacks(makesRequests: false);

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        await builder.Build().RunAsync();
    }
}
