namespace Snippets5.Serialization
{
    using NServiceBus;

    public class BsonSerializerUsage
    {
        public void Simple()
        {
            #region BsonSerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<BsonSerializer>();
            #endregion
        }

    }
}