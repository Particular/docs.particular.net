namespace Snippets6.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
    {
        public void Simple()
        {
            #region XmlSerialization

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }
    }
}