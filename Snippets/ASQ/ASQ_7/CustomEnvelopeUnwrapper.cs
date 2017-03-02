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

        transport.UnwrapMessagesWith(cloudQueueMessage =>
        {
            using (var stream = new MemoryStream(cloudQueueMessage.AsBytes))
            using (var streamReader = new StreamReader(stream))
            using (var textReader = new JsonTextReader(streamReader))
            {
                //try to deserialize to the NServiceBus envelope
                var wrapper = jsonSerializer.Deserialize<MessageWrapper>(textReader);

                if (!string.IsNullOrEmpty(wrapper.Id))
                {
                    //this was a message comming from a NServiceBus endpoint
                    return wrapper;
                }

                //this was a native message just return the body as is with no headers
                return new MessageWrapper
                {
                    Id = cloudQueueMessage.Id,
                    Headers = new Dictionary<string, string>(),
                    Body = cloudQueueMessage.AsBytes
                };
            }
        });

        #endregion
    }

    Newtonsoft.Json.JsonSerializer jsonSerializer = Newtonsoft.Json.JsonSerializer.Create();

}