using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;


var endpointName = "native-integration-asq";
Console.Title = endpointName;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true", useNativeDelayedDeliveries: false);
var routingSettings = endpointConfiguration.UseTransport(transport);
routingSettings.DisablePublishing();

#region Native-message-mapping

transport.MessageUnwrapper = message =>
    message.MessageText.Contains("NativeMessageId") &&
    message.MessageText.Contains("Content")
    ? new MessageWrapper
    {
        Id = message.MessageId,
        Body = message.Body.ToArray(),
        Headers = new Dictionary<string, string>
        {
            { Headers.EnclosedMessageTypes, typeof(NativeMessage).FullName }
        }
    }
    : null; // not a raw native message - allow the framework to deal with it

#endregion

endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));

endpointConfiguration.UsePersistence<LearningPersistence>();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
