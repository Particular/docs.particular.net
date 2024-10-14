using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Hosting;

namespace Sales;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Sales";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Sales");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.UseTransport<LearningTransport>();
        
        builder.UseNServiceBus(endpointConfiguration);

        await builder.Build().RunAsync();
    }
}