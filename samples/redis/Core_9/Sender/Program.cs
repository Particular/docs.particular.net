using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;
using Shared.Messages;

class Program
{
    static Task Main(string[] args)
    {
        Console.Title = "Sender";

        var builder = Host.CreateApplicationBuilder();

        var endpointConfig = new EndpointConfiguration("Sender");
        var transport = endpointConfig.UseTransport(new LearningTransport());
        transport.RouteToEndpoint(typeof(ProcessText), "Receiver");
        endpointConfig.UseSerialization<SystemJsonSerializer>();

        builder.UseNServiceBus(endpointConfig);

        builder.UseRedis("localhost");

        builder.Services.AddHostedService<TextSenderHostedService>();

        var host = builder.Build();

        return host.RunAsync();
    }
}