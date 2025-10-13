using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "RegularEndpoint";

var endpointConfiguration = new EndpointConfiguration("RegularEndpoint");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<SqsTransport>();

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [ENTER] to send a message to the serverless endpoint queue.");
Console.WriteLine("Press any other key to exit.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    await messageSession.Send("ServerlessEndpoint", new TriggerMessage());
    Console.WriteLine("Message sent to the serverless endpoint queue.");
}

await host.StopAsync();