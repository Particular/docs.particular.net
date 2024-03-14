using System;
using System.Collections.Generic;

using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;


var endpointName = "native-integration-asq";
Console.Title = endpointName;

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true", useNativeDelayedDeliveries: false);
var routingSettings = endpointConfiguration.UseTransport(transport);
routingSettings.DisablePublishing();

#region Native-message-mapping

transport.MessageUnwrapper = message => new MessageWrapper
{
    Id = message.MessageId,
    Body = message.Body.ToArray(),
    Headers = new Dictionary<string, string>
            {
                { Headers.EnclosedMessageTypes, typeof(NativeMessage).FullName }
            }
};

#endregion

endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));

endpointConfiguration.UsePersistence<LearningPersistence>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
