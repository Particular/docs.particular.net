namespace Snippets4.Serialization
{
    using NServiceBus;

    public class BinarySerializerUsage
    {
        public void Simple()
        {

            #region BinarySerialization

            Configure.Serialization.Binary();

            #endregion

        }
    }
}