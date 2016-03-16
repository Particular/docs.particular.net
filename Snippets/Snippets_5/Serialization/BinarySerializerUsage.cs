namespace Snippets5.Serialization
{
    using NServiceBus;

    public class BinarySerializerUsage
    {
        public void Simple()
        {

            BusConfiguration busConfiguration = new BusConfiguration();
            #region BinarySerialization

            busConfiguration.UseSerialization<BinarySerializer>();

            #endregion
        }

    }
}