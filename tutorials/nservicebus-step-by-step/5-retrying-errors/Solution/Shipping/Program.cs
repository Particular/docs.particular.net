using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Shipping;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Shipping";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Shipping");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.UseTransport<LearningTransport>();

        builder.UseNServiceBus(endpointConfiguration);

        await builder.Build().RunAsync();
    }
}