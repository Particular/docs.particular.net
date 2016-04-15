namespace Core4.Serialization
{
    using NServiceBus;

    class BinarySerializerUsage
    {
        BinarySerializerUsage()
        {
            #region BinarySerialization

            Configure.Serialization.Binary();

            #endregion

        }
    }
}