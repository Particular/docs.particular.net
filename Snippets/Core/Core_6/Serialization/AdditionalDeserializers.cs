namespace Core6.Serialization
{
    using NServiceBus;

    class AdditionalDeserializers
    {
        AdditionalDeserializers(EndpointConfiguration endpointConfiguration)
        {
            #region AdditionalDeserializers
            // configures XML serialization as default
            endpointConfiguration.UseSerialization<XmlSerializer>();
            // configures additional deserialization
            var jsonSerialization = endpointConfiguration.AddDeserializer<JsonSerializer>();
            jsonSerialization.Settings("serializerSetting");
            #endregion
        }
    }
}
