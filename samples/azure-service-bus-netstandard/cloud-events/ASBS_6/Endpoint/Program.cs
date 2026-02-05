using System;
using System.Text.Json;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Endpoint";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.CloudEvents.Endpoint");
endpointConfiguration.EnableInstallers();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
endpointConfiguration.UseTransport(transport);

#region asb-cloudevents-serialization
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    IncludeFields = true
});
#endregion

#region asb-cloudevents-configuration
var cloudEventsConfiguration = endpointConfiguration.EnableCloudEvents();
#endregion

#region asb-cloudevents-typemapping
cloudEventsConfiguration.TypeMappings["Microsoft.Storage.BlobCreated"] = [typeof(StorageBlobCreatedEventData)];
#endregion


Console.WriteLine("Press any key, the application is starting");
Console.TreatControlCAsInput = true;
var input = Console.ReadKey();
if (input.Key == ConsoleKey.C && (input.Modifiers & ConsoleModifiers.Control) != 0)
{
    Environment.Exit(0);
}
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();

await host.StartAsync();

Console.WriteLine("Press any key to exit");
var key = Console.ReadKey();

await host.StopAsync();
