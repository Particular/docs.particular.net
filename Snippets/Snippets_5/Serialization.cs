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

}