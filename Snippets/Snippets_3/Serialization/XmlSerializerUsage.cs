namespace Snippets3.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
    {
        public void Simple()
        {

            Configure configure = Configure.With();
            #region XmlSerialization

            configure.XmlSerializer();

            #endregion
        }

    }
}