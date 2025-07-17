using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "Phase2";

var builder = Host.CreateApplicationBuilder(args);


var endpointConfiguration = new EndpointConfiguration("ChangeMessageIdentity.Phase2");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region RegisterMessageMutator

endpointConfiguration.RegisterMessageMutator(new MessageIdentityMutator());

#endregion

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Waiting for orders..");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();