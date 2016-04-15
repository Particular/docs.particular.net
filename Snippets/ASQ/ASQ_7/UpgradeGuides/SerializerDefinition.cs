namespace Snippets5.Azure.Transports.AzureStorageQueues.UpgradeGuides
{
    using NServiceBus;

    public class SerializerDefinition
    {
        public SerializerDefinition(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7-serializer-definition

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

            #endregion
        }
    }
}