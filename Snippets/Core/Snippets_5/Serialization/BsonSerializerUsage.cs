namespace Core5.Serialization
{
    using NServiceBus;

    class BsonSerializerUsage
    {
        BsonSerializerUsage(BusConfiguration busConfiguration)
        {
            #region BsonSerialization
            busConfiguration.UseSerialization<BsonSerializer>();
            #endregion
        }

    }
}