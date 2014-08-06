using NServiceBus;


public class Serialization
{
    public void AllTheSerialization()
    {

        #region ConfigureSerializationV4

        Configure.Serialization.Binary();
        Configure.Serialization.Bson();
        Configure.Serialization.Json();
        Configure.Serialization.Xml();

        #endregion
    }

}