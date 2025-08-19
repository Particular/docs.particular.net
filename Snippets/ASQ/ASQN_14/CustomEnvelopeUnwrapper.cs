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
                //Determine whether the message is one expected by the standard program flow.
                // All other messages should be forwarded to the framework by returning null.
                //NOTE: More complex methods may be needed in some scenarios to determine
                // whether the message is of an expected type, but this check
                // should be kept as lightweight as possible. Deserialization of the message
                // body happens later in the pipeline.
                return queueMessage.MessageText.Contains("MyMessageIdFieldName") &&
                       queueMessage.MessageText.Contains("MyMessageCustomPropertyFieldName")
                //this was a native message just return the body as is with no headers
                ? new MessageWrapper
                {
                    Id = queueMessage.MessageId,
                    Headers = [],
                    Body = queueMessage.Body.ToArray()
                }
                : null;
            }
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}