using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "LeftReceiver";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftReceiver");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.EnableInstallers();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
