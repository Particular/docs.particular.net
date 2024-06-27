using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static Task Main(string[] args)
    {
        Console.Title = "Receiver";

        var builder = Host.CreateApplicationBuilder();

        var endpointConfig = new EndpointConfiguration("Receiver");
        endpointConfig.UseTransport(new LearningTransport());
        endpointConfig.UseSerialization<SystemJsonSerializer>();

        builder.UseNServiceBus(endpointConfig);

        var host = builder.Build();
        return host.RunAsync();
    }
}
