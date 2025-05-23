using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var builder = Host.CreateApplicationBuilder(args);

const string endpointName = "Samples-Azure-StorageQueues-Endpoint2";
Console.Title = endpointName;

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true")
{
    QueueNameSanitizer = BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener
};

var routingSettings = endpointConfiguration.UseTransport(transport);
routingSettings.DisablePublishing();

endpointConfiguration.UsePersistence<LearningPersistence>();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();