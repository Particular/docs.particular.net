using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;
using Shared;
using Shared.Messages;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder();

var endpointConfig = new EndpointConfiguration("Sender");
var transport = endpointConfig.UseTransport(new LearningTransport());
transport.RouteToEndpoint(typeof(ProcessText), "Receiver");
endpointConfig.UseSerialization<SystemJsonSerializer>();

#region configure-claim-check
endpointConfig
    .UseClaimCheck(_ => new RedisClaimCheck("localhost"), new SystemJsonClaimCheckSerializer());
endpointConfig
    .Conventions()
    .DefiningClaimCheckPropertiesAs(prop => prop.Name.StartsWith("Large"));
#endregion

builder.UseNServiceBus(endpointConfig);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

const int Megabyte = 1024 * 1024;

#region send-message
await messageSession.Send(new ProcessText { LargeText = GetRandomText(1 * Megabyte) });
#endregion

Console.WriteLine("Sent message with 1MB of random text");

await host.StopAsync();

return;

static string GetRandomText(int length)
{
    return string.Create(length, length, (chars, state) =>
    {
        var random = new Random();
        for (var i = 0; i < state; i++)
        {
            chars[i] = (char)random.Next('a', 'z');
        }
    });
}