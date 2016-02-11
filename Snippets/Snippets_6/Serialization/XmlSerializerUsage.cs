namespace Snippets6.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
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