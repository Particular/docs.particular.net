using NServiceBus;


public class Serialization
{
    public void AllTheSerialization()
    {

        #region ConfigureSerializationV5

        Configure.With(b => b.UseSerialization<Binary>());
        Configure.With(b => b.UseSerialization<Bson>());
        Configure.With(b => b.UseSerialization<Json>());
        Configure.With(b => b.UseSerialization<Xml>());

        #endregion
    }

}