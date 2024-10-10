using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Billing;
class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Billing";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Billing");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.UseTransport<LearningTransport>();

        builder.UseNServiceBus(endpointConfiguration);

        await builder.Build().RunAsync();
    }
}