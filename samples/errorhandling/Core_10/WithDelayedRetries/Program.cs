using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "WithDelayedRetries";

var endpointConfiguration = new EndpointConfiguration("Samples.ErrorHandling.WithDelayedRetries");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Press enter to send a message that will throw an exception.");
Console.WriteLine("Press any key to exit");

while (true)
{
    var key = Console.ReadKey();
    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var myMessage = new MyMessage
    {
        Id = Guid.NewGuid()
    };

    await messageSession.SendLocal(myMessage);
}

await host.StopAsync();
