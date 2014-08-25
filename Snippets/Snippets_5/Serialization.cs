using NServiceBus;


public class Serialization
{
    public void AllTheSerialization()
    {

        #region ConfigureSerializationV5

        var configuration = new BusConfiguration();

        configuration.UseSerialization<BinarySerializer>();
        configuration.UseSerialization<BsonSerializer>();
        configuration.UseSerialization<JsonSerializer>();
        configuration.UseSerialization<XmlSerializer>();

        #endregion
    }

}