using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "Headers";

var endpointConfiguration = new EndpointConfiguration("Samples.Headers");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.RegisterMessageMutator(new MutateIncomingMessages());
endpointConfiguration.RegisterMessageMutator(new MutateIncomingTransportMessages());
endpointConfiguration.RegisterMessageMutator(new MutateOutgoingMessages());
endpointConfiguration.RegisterMessageMutator(new MutateOutgoingTransportMessages());

#region pipeline-config

endpointConfiguration.Pipeline.Register(typeof(IncomingHeaderBehavior), "Manipulates incoming headers");
endpointConfiguration.Pipeline.Register(typeof(OutgoingHeaderBehavior), "Manipulates outgoing headers");

#endregion

#region global-all-outgoing

endpointConfiguration.AddHeaderToAllOutgoingMessages("AllOutgoing", "ValueAllOutgoing");

#endregion

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

#region sending

var myMessage = new MyMessage();
await messageSession.SendLocal(myMessage);

#endregion

await host.StopAsync();