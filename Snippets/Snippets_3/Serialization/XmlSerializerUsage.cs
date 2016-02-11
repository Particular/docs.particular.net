namespace Snippets3.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
    {
        public void Simple()
        {

            #region XmlSerialization

            Configure configure = Configure.With();
            configure.XmlSerializer();

            #endregion
        }

    }
}