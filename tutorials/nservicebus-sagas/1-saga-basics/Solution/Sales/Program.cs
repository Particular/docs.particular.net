using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Sales");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");

// Decrease the default delayed delivery interval so that we don't
// have to wait too long for the message to be moved to the error queue
var recoverability = endpointConfiguration.Recoverability();
recoverability.Delayed(delayed => delayed.TimeIncrease(TimeSpan.FromSeconds(2)));

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();