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

        #region BinarySerialization

        Configure.Serialization.Binary();
        
        #endregion

        #region BsonSerialization

        Configure.Serialization.Bson();

        #endregion

        #region JsonSerialization

        Configure.Serialization.Json();
        
        #endregion

        #region XmlSerialization

        Configure.Serialization.Xml();

        #endregion
    }

}