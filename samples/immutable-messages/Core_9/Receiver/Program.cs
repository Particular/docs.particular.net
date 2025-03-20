using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.ApplyCustomConventions();
Console.WriteLine("Samples.ImmutableMessages.UsingInterfaces.Receiver started. Press any key to exit.");
Console.ReadKey();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
