using NServiceBus;


public class Serialization
{
    public void AllTheSerialization()
    {

        #region ConfigureSerializationV5

        Configure.With(builder => builder.UseSerialization<Binary>());
        Configure.With(builder => builder.UseSerialization<Bson>());
        Configure.With(builder => builder.UseSerialization<Json>());
        Configure.With(builder => builder.UseSerialization<Xml>());

        #endregion
    }

}