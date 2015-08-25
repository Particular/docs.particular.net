namespace Snippets3.Serialization
{
    using NServiceBus;

    public class XmlSerialization
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