using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "Bridge";

var builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = false;
    options.SingleLine = true;
    options.TimestampFormat = "hh:mm:ss ";
});

var bridgeConfiguration = new BridgeConfiguration();

#region ServiceControlTransport

var serviceControlTransport = new BridgeTransport(new LearningTransport());
serviceControlTransport.HasEndpoint("Particular.ServiceControl");
serviceControlTransport.HasEndpoint("Particular.Monitoring");
serviceControlTransport.HasEndpoint("error");
serviceControlTransport.HasEndpoint("audit");

#endregion

#region EndpointSideConfig

var learningTransport = new LearningTransport
{
    // Set storage directory and add the character '2' to simulate a different transport.
    StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
};

var endpointsTransport = new BridgeTransport(learningTransport)
{
    // A different name is required if transports are used twice.
    Name = "right-side"
};

endpointsTransport.HasEndpoint("Samples.Bridge.Endpoint");

#endregion

bridgeConfiguration.AddTransport(serviceControlTransport);
bridgeConfiguration.AddTransport(endpointsTransport);
builder.UseNServiceBusBridge(bridgeConfiguration);

var host = builder.Build();

await host.RunAsync();