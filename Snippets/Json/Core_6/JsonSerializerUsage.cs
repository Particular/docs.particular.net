namespace Core6
{
    using System.Text;
    using NServiceBus;

    class JsonSerializerUsage
    {
        JsonSerializerUsage(EndpointConfiguration endpointConfiguration)
        {
            #region JsonSerialization

            endpointConfiguration.UseSerialization<JsonSerializer>();

            #endregion

            #region JsonSerializationEncoding

            var noBomEncoding = new UTF8Encoding(false);

            endpointConfiguration.UseSerialization<JsonSerializer>()
                .Encoding(noBomEncoding);

            #endregion
        }
    }
}