namespace Snippets5.Serialization
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