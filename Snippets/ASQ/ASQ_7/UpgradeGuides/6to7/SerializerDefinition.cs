using NServiceBus;

class SerializerDefinition
{
    SerializerDefinition(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7-serializer-definition

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

        #endregion
    }
}
