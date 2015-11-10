namespace Snippets5.Serialization
{
    using NServiceBus;

    public class BinarySerialization
    {
        public void Simple()
        {

            #region BinarySerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<BinarySerializer>();

            #endregion
        }

    }
}