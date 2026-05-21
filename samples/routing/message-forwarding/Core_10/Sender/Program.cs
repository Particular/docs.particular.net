using System;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sender";

#region route-message-to-original-destination

var endpointConfiguration = new EndpointConfiguration("Sender");
var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(typeof(ImportantMessage), "OriginalDestination");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Endpoint Started. Press s to send a very important message. Any other key to exit");

var i = 0;

while (Console.ReadKey(true).Key == ConsoleKey.S)
{
    var message = new ImportantMessage
    {
        Text = $"Hello there: {i++}"
    };
    await messageSession.Send(message);
    Console.WriteLine("Message sent");
}

await host.StopAsync();