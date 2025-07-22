using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "FeaturesSample";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Features");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

Console.WriteLine("Press 'H' to send a HandlerMessage");
Console.WriteLine("Press 'S' to send a StartSagaMessage");
Console.WriteLine("Press any other key to exit");

// Start the host in the background
var hostTask = app.RunAsync();

// Get the message session from DI
var messageSession = app.Services.GetRequiredService<IMessageSession>();

// Handle console input in main thread
while (true)
{
    var key = Console.ReadKey();

    if (key.Key == ConsoleKey.H)
    {
        await SendHandlerMessage(messageSession);
        continue;
    }
    if (key.Key == ConsoleKey.S)
    {
        await SendSagaMessage(messageSession);
        continue;
    }

    // Any other key exits
    break;
}

// Stop the host gracefully
await app.StopAsync();

static Task SendHandlerMessage(IMessageSession messageSession)
{
    Console.WriteLine();
    Console.WriteLine("HandlerMessage sent");
    var message = new HandlerMessage();
    return messageSession.SendLocal(message);
}

static Task SendSagaMessage(IMessageSession messageSession)
{
    Console.WriteLine();
    Console.WriteLine("StartSagaMessage sent");
    var message = new StartSagaMessage
    {
        SentTime = DateTimeOffset.UtcNow,
        TheId = Guid.NewGuid()
    };
    return messageSession.SendLocal(message);
}