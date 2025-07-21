using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "DispatchNotification";

var builder = Host.CreateApplicationBuilder(args);
#region endpoint-configuration
var endpointConfiguration = new EndpointConfiguration("Samples.DispatchNotification");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.NotifyDispatch(new SampleDispatchNotifier());
#endregion

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetService<IMessageSession>();
Console.WriteLine("Press any key to send a message. Press 'Escape' to exit.");
while (true) 
{ 
    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.Escape) break;

    await messageSession.SendLocal(new SomeMessage());
}

await host.StopAsync();