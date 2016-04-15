namespace Snippets6.Serialization
{
    using NServiceBus;

    class JsonSerializerUsage
    {
        JsonSerializerUsage(EndpointConfiguration endpointConfiguration)
        {
            #region JsonSerialization

            endpointConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }
    }
}