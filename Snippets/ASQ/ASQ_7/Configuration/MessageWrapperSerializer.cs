namespace ASQ_7.Configuration
{
    using NServiceBus;

    public class MessageWrapperSerializer
    {
        

        public void ConfigureEndpoints()
        {
            #region SerializerAndMessageWrapperSerializer

            var endpoint = new EndpointConfiguration("ASQ-Endpoint");
            // serialize the messages using the XML serializer:
            endpoint.UseSerialization<XmlSerializer>();
            endpoint.UseTransport<AzureStorageQueueTransport>()
                .ConnectionStringName("ASQ-ConnectionString")
                // wrap messages in JSON
                .SerializeMessageWrapperWith<JsonSerializer>();

            #endregion
        }
    }
}