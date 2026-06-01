using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "OriginalDestination";

#region forward-messages-after-processing

var endpointConfiguration = new EndpointConfiguration("OriginalDestination");
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.ForwardMessagesAfterProcessingTo("UpgradedDestination");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
await builder.Build().StartAsync();
