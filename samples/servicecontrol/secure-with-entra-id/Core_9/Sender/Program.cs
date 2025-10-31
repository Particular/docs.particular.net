using System;
using System.IO;
using NServiceBus;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

Console.Title = "Sender";
var endpointConfiguration = new EndpointConfiguration("SecureWithEntraID.Sender");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var transportConfiguration = endpointConfiguration.UseTransport<LearningTransport>();
transportConfiguration.StorageDirectory(Path.Combine(".learningtransport"));
var routing = transportConfiguration.Routing();
routing.RouteToEndpoint(typeof(SimpleRequest), "SecureWithEntraID.Receiver");

endpointConfiguration.OnEndpointStarted(async messageSession =>
{
    var simpleMessage = new SimpleRequest
    {
        Text = "Hi, there! — " + DateTime.Now.ToLongTimeString()
    };

    await messageSession.Send(simpleMessage);

    Console.WriteLine($"Sent a new message with Text = {simpleMessage.Text}.");
    Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
});

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();