using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;
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

        #region configure-claim-check
        endpointConfig.UseClaimCheck(
            _ => new RedisClaimCheck("localhost"),
            new SystemJsonClaimCheckSerializer()
        );
        endpointConfig.Conventions()
            .DefiningClaimCheckPropertiesAs(
                prop => prop.Name.StartsWith("Large")
            );
        #endregion

        builder.UseNServiceBus(endpointConfig);

        builder.Services.AddHostedService<TextSenderHostedService>();

        var host = builder.Build();

        return host.RunAsync();
    }
}