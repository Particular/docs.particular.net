namespace Core3.Serialization
{
    using NServiceBus;

    class BinarySerializerUsage
    {
        BinarySerializerUsage(Configure configure)
        {
            #region BinarySerialization

            configure.BinarySerializer();

            #endregion
        }
    }
}