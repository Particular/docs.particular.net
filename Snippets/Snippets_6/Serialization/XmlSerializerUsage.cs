namespace Snippets6.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region XmlSerialization

            endpointConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }
    }
}