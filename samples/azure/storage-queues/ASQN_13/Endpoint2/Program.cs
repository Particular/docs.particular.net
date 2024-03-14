using System;

using NServiceBus;

var endpointName = "Samples-Azure-StorageQueues-Endpoint2";
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

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();

