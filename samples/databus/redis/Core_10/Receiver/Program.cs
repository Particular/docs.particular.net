using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;
using Shared;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Receiver";

        var builder = Host.CreateApplicationBuilder();

        var endpointConfig = new EndpointConfiguration("Receiver");
        endpointConfig.UseTransport(new LearningTransport());
        endpointConfig.UseSerialization<SystemJsonSerializer>();
        endpointConfig.UseClaimCheck(_ => new RedisClaimCheck("localhost"), new SystemJsonClaimCheckSerializer());
        endpointConfig.Conventions().DefiningClaimCheckPropertiesAs(prop => prop.Name.StartsWith("Large"));

        builder.UseNServiceBus(endpointConfig);

        var host = builder.Build();

        await host.RunAsync();
    }
}
