using NServiceBus;
using NServiceBus.Utf8Json;
using Utf8Json.Resolvers;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Utf8JsonSerialization

        endpointConfiguration.UseSerialization<Utf8JsonSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region Utf8JsonResolver

        var serialization = endpointConfiguration.UseSerialization<Utf8JsonSerializer>();
        serialization.Resolver(StandardResolver.SnakeCase);

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region Utf8JsonContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<Utf8JsonSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }
}