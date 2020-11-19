using NServiceBus;

class MessageWrapperSerializer
{

    MessageWrapperSerializer(EndpointConfiguration endpointConfiguration)
    {
        #region SerializerAndMessageWrapperSerializer

        // serialize the messages using the XML serializer:
        endpointConfiguration.UseSerialization<XmlSerializer>();
        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        // wrap messages in JSON
        transport.SerializeMessageWrapperWith<NewtonsoftSerializer>();

        #endregion
    }
}