using NServiceBus;
using NServiceBus.Hyperion;
using Hyperion;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region HyperionSerialization

        endpointConfiguration.UseSerialization<HyperionSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region HyperionCustomSettings

        var options = new SerializerOptions(
            preserveObjectReferences: true);
        var serialization = endpointConfiguration.UseSerialization<HyperionSerializer>();
        serialization.Options(options);

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region HyperionContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<HyperionSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}