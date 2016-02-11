namespace Snippets4.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
    {
        public void Simple()
        {

            #region XmlSerialization

            Configure.Serialization.Xml();

            #endregion
        }

    }
}