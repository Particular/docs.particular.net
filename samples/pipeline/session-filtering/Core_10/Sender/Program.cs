using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder();

#region register-session-key-provider
builder.Services.AddSingleton<ISessionKeyProvider, RotatingSessionKeyProvider>();
#endregion

var endpointConfiguration = new EndpointConfiguration("Sender");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.ApplySessionFilter();

var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(
    typeof(SomeMessage),
    "Receiver"
);

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

using var host = builder.Build();

await host.StartAsync();

var sessionKeyProvider = host.Services.GetRequiredService<ISessionKeyProvider>();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
PrintMenu(sessionKeyProvider);

var index = 1;

while (true)
{
    var key = Console.ReadKey(true).Key;

    if (key == ConsoleKey.Q)
    {
        break;
    }

    if (key == ConsoleKey.C)
    {
        sessionKeyProvider.NextKey();
        PrintMenu(sessionKeyProvider);
        continue;
    }


    await messageSession.Send(new SomeMessage { Counter = index });
    Console.WriteLine($"Sent message {index++}");
}

await host.StopAsync();

static void PrintMenu(ISessionKeyProvider sessionKeyProvider)
{
    Console.Clear();
    Console.WriteLine($"Session Key: {sessionKeyProvider.SessionKey}");
    Console.WriteLine("C) Change Session Key");
    Console.WriteLine("Q) Close");
    Console.WriteLine("any other key to send a message");
}
