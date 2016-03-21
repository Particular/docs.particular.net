namespace Snippets4.Serialization
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