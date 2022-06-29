﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;
using NServiceBus.Features;

class Program
{
    static async Task Main()
    {
        var endpointName = "native-integration-asq";
        Console.Title = endpointName;

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");
        transport.DisablePublishing();
        transport.DelayedDelivery().DisableDelayedDelivery();

        #region Native-message-mapping

        transport.UnwrapMessagesWith(message => new MessageWrapper
        {
            Id = message.MessageId,
            Body = message.Body.ToArray(),
            Headers = new Dictionary<string, string>
            {
                { Headers.EnclosedMessageTypes, typeof(NativeMessage).FullName }
            }
        });

        #endregion

        endpointConfiguration.DisableFeature<TimeoutManager>();
        endpointConfiguration.UsePersistence<LearningPersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
