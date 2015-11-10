namespace Snippets4.Serialization
{
    using NServiceBus;

    public class XmlSerialization
    {
        public void Simple()
        {

            #region XmlSerialization

            Configure.Serialization.Xml();

            #endregion
        }

    }
}