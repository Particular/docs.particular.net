namespace Core4
{
    using NServiceBus;

    class BsonSerializerUsage
    {
        BsonSerializerUsage()
        {
            #region BsonSerialization

            Configure.Serialization.Bson();

            #endregion
        }

    }
}