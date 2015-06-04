namespace Snippets5
{
    using NServiceBus;

    public class Serialization
    {
        public void AllTheSerialization()
        {

            #region ConfigureSerialization

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UseSerialization<BinarySerializer>();
            busConfiguration.UseSerialization<BsonSerializer>();
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }
        public void JsonSerialization()
        {

            #region JsonSerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }
        public void BinarySerialization()
        {

            #region BinarySerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<BinarySerializer>();

            #endregion
        }
        public void BsonSerialization()
        {
            #region BsonSerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<BsonSerializer>();
            #endregion
        }

        public void XmlSerialization()
        {
            #region XmlSerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }

    }
}