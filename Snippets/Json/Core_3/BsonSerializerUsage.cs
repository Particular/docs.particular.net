namespace Core3
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