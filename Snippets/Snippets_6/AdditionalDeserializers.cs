namespace Snippets6
{
    using NServiceBus;

    public class AdditionalDeserializers
    {
        void RegisterSerializers()
        {
            #region AdditionalDeserializers
            EndpointConfiguration configuration = new EndpointConfiguration();
            // configures XML serialization as default
            configuration.UseSerialization<XmlSerializer>();
            // configures additional deserialization
            configuration.AddDeserializer<JsonSerializer>();
            #endregion
        }
    }
}
