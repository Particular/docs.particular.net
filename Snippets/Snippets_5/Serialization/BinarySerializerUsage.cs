namespace Snippets5.Serialization
{
    using NServiceBus;

    public class BinarySerializerUsage
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