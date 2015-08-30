namespace Snippets6
{
    using NServiceBus;
    using NServiceBus.Features;

    public class AdditionalDeserializers
    {
        void RegisterSerializers()
        {
            #region AdditionalDeserializers
            BusConfiguration busConfiguration = new BusConfiguration();
            // configures XML serialization as default
            busConfiguration.UseSerialization<XmlSerializer>()
            // configures additional deserialization
            busConfiguration.AddDeserializer<JsonSerializer>();
            busConfiguration.AddDeserializer<BsonSerializer>();
            #endregion
        }
    }
}
