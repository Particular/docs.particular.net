using NServiceBus;

class XmlSerializerUsage
{
    public XmlSerializerUsage(EndpointConfiguration endpointConfiguration)
    {
        #region XmlSerialization

        endpointConfiguration.UseSerialization<XmlSerializer>();

        #endregion
    }
}