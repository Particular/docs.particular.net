using NServiceBus;


public class Serialization
{
    public void AllTheSerialization()
    {

        #region ConfigureSerialization

        BusConfiguration configuration = new BusConfiguration();

        configuration.UseSerialization<BinarySerializer>();
        configuration.UseSerialization<BsonSerializer>();
        configuration.UseSerialization<JsonSerializer>();
        configuration.UseSerialization<XmlSerializer>();

        #endregion
    }
    public void JsonSerialization()
    {

        #region JsonSerialization

        BusConfiguration configuration = new BusConfiguration();
        configuration.UseSerialization<JsonSerializer>();

        #endregion
    }
    public void BinarySerialization()
    {

        #region BinarySerialization

        BusConfiguration configuration = new BusConfiguration();
        configuration.UseSerialization<BinarySerializer>();

        #endregion
    }
    public void BsonSerialization()
    {
        #region BsonSerialization

        BusConfiguration configuration = new BusConfiguration();
        configuration.UseSerialization<BsonSerializer>();
        #endregion
    }

    public void XmlSerialization()
    {
        #region XmlSerialization

        BusConfiguration configuration = new BusConfiguration();
        configuration.UseSerialization<XmlSerializer>();

        #endregion
    }

}