using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("RetryFailedMessages.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
routing.RouteToEndpoint(typeof(SimpleMessage), "RetryFailedMessages.Receiver");

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [Enter] to send a new message. Press any other key to finish.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var simpleMessage = new SimpleMessage();

    await messageSession.Send(simpleMessage);

    Console.WriteLine("Press [Enter] to send a new message. Press any other key to finish.");
}

await host.StopAsync();