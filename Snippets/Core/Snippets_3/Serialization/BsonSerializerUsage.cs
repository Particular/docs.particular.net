namespace Core3.Serialization
{
    using NServiceBus;

    class BsonSerializerUsage
    {
        BsonSerializerUsage(Configure configure)
        {
            #region BsonSerialization

            configure.BsonSerializer();

            #endregion
        }
    }
}