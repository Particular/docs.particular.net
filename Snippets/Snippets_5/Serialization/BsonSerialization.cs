namespace Snippets5
{
    using NServiceBus;

    public class BsonSerialization
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