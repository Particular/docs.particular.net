using NServiceBus;

namespace ASQ_7.UpgradeGuides._6to7
{
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
}