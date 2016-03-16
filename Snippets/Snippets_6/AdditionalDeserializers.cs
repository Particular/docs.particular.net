namespace Snippets6
{
    using NServiceBus;

    public class AdditionalDeserializers
    {
        void RegisterSerializers()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region AdditionalDeserializers
            // configures XML serialization as default
            endpointConfiguration.UseSerialization<XmlSerializer>();
            // configures additional deserialization
            endpointConfiguration.AddDeserializer<JsonSerializer>();
            #endregion
        }
    }
}
