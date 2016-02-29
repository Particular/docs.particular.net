namespace Snippets6
{
    using NServiceBus;

    public class AdditionalDeserializers
    {
        void RegisterSerializers()
        {
            #region AdditionalDeserializers
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            // configures XML serialization as default
            endpointConfiguration.UseSerialization<XmlSerializer>();
            // configures additional deserialization
            endpointConfiguration.AddDeserializer<JsonSerializer>();
            #endregion
        }
    }
}
