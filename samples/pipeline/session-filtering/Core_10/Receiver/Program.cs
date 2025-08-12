using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<ISessionKeyProvider, RotatingSessionKeyProvider>();

var endpointConfiguration = new EndpointConfiguration("Receiver");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.ApplySessionFilter();

endpointConfiguration.UseTransport(new LearningTransport());


builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var sessionKeyProvider = host.Services.GetRequiredService<ISessionKeyProvider>();
PrintMenu(sessionKeyProvider);

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
    }
}

await host.StopAsync();

static void PrintMenu(ISessionKeyProvider sessionKeyProvider)
{
    Console.Clear();
    Console.WriteLine($"Session Key: {sessionKeyProvider.SessionKey}");
    Console.WriteLine("C) Change Session Key");
    Console.WriteLine("Q) Close");
}
