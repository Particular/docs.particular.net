using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;

class Program
{
    static async Task Main()
    {
        var endpointName = "native-integration-asq";
        Console.Title = endpointName;

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();

        var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true", useNativeDelayedDeliveries: false);
        // TODO: ASQ transport is not yet ready, Core v8 work is still in progress
        // transport.DisablePublishing();

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

        endpointConfiguration.UseTransport(transport);
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
