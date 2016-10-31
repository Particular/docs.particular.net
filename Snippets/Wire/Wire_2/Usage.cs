using NServiceBus;
using NServiceBus.Wire;
using Wire;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region WireSerialization

        endpointConfiguration.UseSerialization<WireSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region WireCustomSettings

        var options = new SerializerOptions(
            preserveObjectReferences: true);
        var serialization = endpointConfiguration.UseSerialization<WireSerializer>();
        serialization.Options(options);

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region WireContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<WireSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}