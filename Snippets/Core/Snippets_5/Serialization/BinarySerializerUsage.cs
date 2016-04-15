namespace Core5.Serialization
{
    using NServiceBus;

    class BinarySerializerUsage
    {
        BinarySerializerUsage(BusConfiguration busConfiguration)
        {
            #region BinarySerialization

            busConfiguration.UseSerialization<BinarySerializer>();

            #endregion
        }

    }
}