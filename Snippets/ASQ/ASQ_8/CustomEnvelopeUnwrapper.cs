﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;

class CustomEnvelopeUnwrapper
{

    CustomEnvelopeUnwrapper(EndpointConfiguration endpointConfiguration)
    {
        #region CustomEnvelopeUnwrapper 7.1

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

        transport.UnwrapMessagesWith(cloudQueueMessage =>
        {
            using (var stream = new MemoryStream(cloudQueueMessage.AsBytes))
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