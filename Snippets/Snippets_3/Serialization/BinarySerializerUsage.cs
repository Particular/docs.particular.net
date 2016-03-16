namespace Snippets3.Serialization
{
    using NServiceBus;

    public class BinarySerializerUsage
    {
        public void Simple()
        {

            Configure configure = Configure.With();
            #region BinarySerialization

            configure.BinarySerializer();

            #endregion

        }
    }
}