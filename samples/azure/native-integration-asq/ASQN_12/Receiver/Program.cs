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
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
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

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}
