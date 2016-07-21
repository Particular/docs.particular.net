namespace Core6
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
            endpointConfiguration.AddDeserializer<JsonSerializer>().Settings("serializerSetting");
            #endregion
        }
    }
}
