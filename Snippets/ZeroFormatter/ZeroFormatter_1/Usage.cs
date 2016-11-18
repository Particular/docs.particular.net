using NServiceBus;
using NServiceBus.ZeroFormatter;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region ZeroFormatterSerialization

        endpointConfiguration.UseSerialization<ZeroSerializer>();

        #endregion
    }


    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region ZeroFormatterContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<ZeroSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}