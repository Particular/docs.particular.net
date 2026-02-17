using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Sales");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region NoDelayedRetries
var recoverability = endpointConfiguration.Recoverability();
recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
#endregion

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();