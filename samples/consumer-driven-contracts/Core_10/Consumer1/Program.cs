using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Consumer1";

        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Consumer1");
        var transport = endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        await builder.Build().RunAsync();
    }
}