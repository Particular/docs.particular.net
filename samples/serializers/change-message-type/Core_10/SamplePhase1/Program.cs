using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Phase1";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ChangeMessageIdentity.Phase1");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();

var newOrder = new CreateOrderPhase1
{
    OrderDate = DateTime.Now
};
var messageSession = host.Services.GetRequiredService<IMessageSession>();

await messageSession.Send("ChangeMessageIdentity.Phase2", newOrder);

Console.WriteLine("CreateOrderPhase1 Sent");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();
