using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Sender";

var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.Sender");
endpointConfiguration.EnableInstallers();
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());
routing.RouteToEndpoint(typeof(SimpleMessage), "FixMalformedMessages.Receiver");

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
while (true)
{
    var key = Console.ReadKey();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var guid = Guid.NewGuid();

    var simpleMessage = new SimpleMessage
    {
        Id = guid.ToString().ToLowerInvariant()
    };
    await messageSession.Send(simpleMessage);

    Console.WriteLine($"Sent a new message with Id = {guid}.");

    Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
}

await host.StopAsync();
