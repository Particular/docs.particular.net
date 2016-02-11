namespace Snippets6.Serialization
{
    using NServiceBus;

    public class XmlSerialization
    {
        public void Simple()
        {
            #region XmlSerialization

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseSerialization<XmlSerializer>();

            #endregion
        }
    }
}