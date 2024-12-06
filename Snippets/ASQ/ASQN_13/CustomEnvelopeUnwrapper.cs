using System.Buffers.Text;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;

class CustomEnvelopeUnwrapper
{

    CustomEnvelopeUnwrapper(EndpointConfiguration endpointConfiguration)
    {
        #region CustomEnvelopeUnwrapper

        var transport = new AzureStorageQueueTransport("connection string")
        {
            MessageUnwrapper = queueMessage =>
            {
                //This sample expects the native message to be serialized only,
                // conforming to the specified endpoint serializer.
                //NServiceBus messages will always be Base64 encoded, so any messages
                // of this type can be forwarded to the framework by returning null.
                return Base64.IsValid(queueMessage.MessageText)
                ? null
                //this was a native message just return the body as is with no headers
                : new MessageWrapper 
                {
                    Id = queueMessage.MessageId,
                    Headers = [],
                    Body = queueMessage.Body.ToArray()
                };
            }
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}