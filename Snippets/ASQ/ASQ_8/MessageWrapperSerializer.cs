using NServiceBus;
using JsonSerializer = NServiceBus.JsonSerializer;

class MessageWrapperSerializer
{

    MessageWrapperSerializer(EndpointConfiguration endpointConfiguration)
    {
        #region SerializerAndMessageWrapperSerializer

        // serialize the messages using the XML serializer:
        endpointConfiguration.UseSerialization<XmlSerializer>();
        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionStringName("ASQ-ConnectionString");
        // wrap messages in JSON
        transport.SerializeMessageWrapperWith<JsonSerializer>();

        #endregion
    }
}