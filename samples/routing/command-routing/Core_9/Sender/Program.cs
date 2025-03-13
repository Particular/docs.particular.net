using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

var endpointConfiguration = new EndpointConfiguration("Samples.CommandRouting.Sender");

#region configure-command-route
var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(
    messageType: typeof(PlaceOrder),
    destination: "Samples.CommandRouting.Receiver"
);
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
