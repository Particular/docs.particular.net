using NServiceBus;

class MessageWrapperSerializer
{

    MessageWrapperSerializer(EndpointConfiguration endpointConfiguration)
    {
        #region SerializerAndMessageWrapperSerializer

        // serialize the messages using the XML serializer:
        endpointConfiguration.UseSerialization<XmlSerializer>();

        var transport = new AzureStorageQueueTransport("connection string")
        {
            // wrap messages in JSON
            MessageWrapperSerializationDefinition = new NewtonsoftSerializer()
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}