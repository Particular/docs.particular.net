namespace Core5
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