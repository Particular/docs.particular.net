using Messages;
using Microsoft.Extensions.Hosting;

var endpointName = "Shipping";

Console.Title = endpointName;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());
routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");
routing.RouteToEndpoint(typeof(ShipWithAlpine), "Shipping");

endpointConfiguration.UsePersistence<LearningPersistence>();

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");

// Decrease the default delayed delivery interval so that we don't
// have to wait too long for the message to be moved to the error queue
var recoverability = endpointConfiguration.Recoverability();
recoverability.Delayed(delayed => delayed.TimeIncrease(TimeSpan.FromSeconds(2)));

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();