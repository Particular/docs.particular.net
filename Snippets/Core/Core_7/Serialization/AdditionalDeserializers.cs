namespace Core7.Serialization
{
    using NServiceBus;

    class AdditionalDeserializers
    {
        AdditionalDeserializers(EndpointConfiguration endpointConfiguration)
        {
            #region AdditionalDeserializers
            // Configures new default serialization
            var mySerializer = endpointConfiguration.UseSerialization<MyCustomSerializerDefinition>();
            mySerializer.Settings("serializerSetting");
            // Configures additional deserializer, like the previous (default) serializer to be compatible with in-flight messages.
            endpointConfiguration.AddDeserializer<XmlSerializer>();
            #endregion
        }
    }
}
