using System.Text.Json;
using NServiceBus;

class SystemJsonSerializerUsage
{
    SystemJsonSerializerUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SystemJsonSerialization

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        #endregion


        #region SystemJsonContentType

        endpointConfiguration.UseSerialization<SystemJsonSerializer>()
            .ContentType("application/json; systemjson");

        #endregion

        #region SystemJsonOptions

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        endpointConfiguration.UseSerialization<SystemJsonSerializer>()
            .Options(jsonSerializerOptions);

        #endregion
    }
}