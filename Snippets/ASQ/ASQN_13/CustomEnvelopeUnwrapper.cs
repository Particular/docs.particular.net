using System;
using System.Collections.Generic;
using System.Text.Json;
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
                var messageText = Convert.FromBase64String(queueMessage.MessageText);

                //try deserialize to a NServiceBus envelope first
                var wrapper = JsonSerializer.Deserialize<MessageWrapper>(messageText);

                if (wrapper?.Id != null)
                {
                    //this was a envelope message
                    return wrapper;
                }

                //this was a native message just return the body as is with no headers
                return new MessageWrapper
                {
                    Id = queueMessage.MessageId,
                    Headers = new Dictionary<string, string>(),
                    Body = messageText
                };
            }
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}