using NServiceBus;


public class Serialization
{
    public void AllTheSerialization()
    {

        #region ConfigureSerialization

        Configure.Serialization.Binary();
        Configure.Serialization.Bson();
        Configure.Serialization.Json();
        Configure.Serialization.Xml();

        #endregion
    }

}