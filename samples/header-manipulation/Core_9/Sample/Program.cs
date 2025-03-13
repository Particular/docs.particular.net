using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;
using Sample;


Console.Title = "Headers";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
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


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();