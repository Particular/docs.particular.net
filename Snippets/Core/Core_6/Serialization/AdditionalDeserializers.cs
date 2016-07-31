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
            var mySerializer = endpointConfiguration.AddDeserializer<MyCustomSerializerDefinition>();
            mySerializer.Settings("serializerSetting");
            #endregion
        }
    }
}
