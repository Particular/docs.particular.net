using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using UsingClasses.Messages;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UsePersistence<LearningPersistence>();

var routingConfiguration = endpointConfiguration.UseTransport(new LearningTransport());
routingConfiguration.RouteToEndpoint(typeof(MyMessageImpl), "Samples.ImmutableMessages.UsingInterfaces.Receiver");
routingConfiguration.RouteToEndpoint(typeof(MyMessage), "Samples.ImmutableMessages.UsingInterfaces.Receiver");

endpointConfiguration.ApplyCustomConventions();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'C' to send a message (using a class with a non-default constructor");
Console.WriteLine("Press 'I' to send a message (using a private class that implements a shared interface");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.C && key.Key != ConsoleKey.I)
    {
        break;
    }

    if (key.Key == ConsoleKey.C)
    {
        await SendMessageAsClass(messageSession);
        continue;
    }

    if (key.Key == ConsoleKey.I)
    {
        await SendMessageAsInterface(messageSession);
    }
}

await host.StopAsync();
return;

static Task SendMessageAsClass(IMessageSession messageSession)
{
    var data = Guid.NewGuid().ToString();

    Console.WriteLine($"Message sent, data: {data}");
    var myMessage = new MyMessage(data);
    return messageSession.Send(myMessage);
}

static Task SendMessageAsInterface(IMessageSession messageSession)
{
    var data = Guid.NewGuid().ToString();

    Console.WriteLine($"Message sent, data: {data}");
    #region immutable-messages-as-interface-sending
    var myMessage = new MyMessageImpl()
    {
        Data = data
    };
    return messageSession.Send(myMessage);
    #endregion
}