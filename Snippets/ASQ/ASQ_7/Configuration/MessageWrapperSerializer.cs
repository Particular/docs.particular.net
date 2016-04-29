namespace ASQ_7.Configuration
{
    using NServiceBus;

    public class MessageWrapperSerializer
    {
        

        public void ConfigureEndpoints()
        {
            #region SerializerAndMessageWrapperSerializer
            var endpointA = new EndpointConfiguration("Endpoint-A");
            // serialize the messages using the XML serializer:
            endpointA.UseSerialization<XmlSerializer>();
            // enables handling messages from Endpoint-B
            endpointA.AddDeserializer<JsonSerializer>();
            endpointA.UseTransport<AzureStorageQueueTransport>()
                .ConnectionStringName("ASQ-ConnectionString")
                // wrap messages in JSON to maintain compatibility with existing messages
                .SerializeMessageWrapperWith<JsonSerializer>();

            var endpointB = new EndpointConfiguration("Endpoint-B");
            // serialize the messages using the JSON serializer:
            endpointB.UseSerialization<JsonSerializer>();
            // enables handling messages from Endpoint-A
            endpointB.AddDeserializer<XmlSerializer>();
            // wraps messages in JSON since this is the configured message serializer
            endpointB.UseTransport<AzureStorageQueueTransport>()
                .ConnectionStringName("ASQ-ConnectionString");
            #endregion
        }
    }
}