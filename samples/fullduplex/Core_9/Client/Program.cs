using System;
using NServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

Console.Title = "Client";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.FullDuplex.Client");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press enter to send a message");

#region ClientLoop

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }
    var guid = Guid.NewGuid();
    Console.WriteLine($"Requesting to get data by id: {guid:N}");

    var message = new RequestDataMessage
    {
        DataId = guid,
        String = "String property value"
    };
    await messageSession.Send("Samples.FullDuplex.Server", message);
}

#endregion

await host.StopAsync();