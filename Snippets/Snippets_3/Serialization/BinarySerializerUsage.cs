namespace Snippets3.Serialization
{
    using NServiceBus;

    public class BinarySerializerUsage
    {
        public void Simple()
        {

            #region BinarySerialization

            Configure configure = Configure.With();
            configure.BinarySerializer();

            #endregion

        }
    }
}