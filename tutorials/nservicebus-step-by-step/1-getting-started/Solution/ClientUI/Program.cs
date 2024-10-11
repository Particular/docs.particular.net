using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;

namespace ClientUI;
class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "ClientUI";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("ClientUI");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var transport = endpointConfiguration.UseTransport(new LearningTransport());

        builder.UseNServiceBus(endpointConfiguration);

        await builder.Build().RunAsync();
    }
}