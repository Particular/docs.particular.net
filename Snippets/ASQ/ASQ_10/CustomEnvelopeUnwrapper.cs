using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;

class CustomEnvelopeUnwrapper
{

    CustomEnvelopeUnwrapper(EndpointConfiguration endpointConfiguration)
    {
        #region CustomEnvelopeUnwrapper

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

        transport.UnwrapMessagesWith(queueMessage =>
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(queueMessage.MessageText)))
            using (var streamReader = new StreamReader(stream))
            using (var textReader = new JsonTextReader(streamReader))
            {
                //try deserialize to a NServiceBus envelope first
                var wrapper = jsonSerializer.Deserialize<MessageWrapper>(textReader);

                if (wrapper.Id != null)
                {
                    //this was a envelope message
                    return wrapper;
                }

                //this was a native message just return the body as is with no headers
                return new MessageWrapper
                {
                    Id = queueMessage.MessageId,
                    Headers = new Dictionary<string, string>(),
                    Body = Convert.FromBase64String(queueMessage.MessageText)
                };
            }
        });

        #endregion
    }

    Newtonsoft.Json.JsonSerializer jsonSerializer = Newtonsoft.Json.JsonSerializer.Create();

}