namespace Core8
{
    using NServiceBus;

    class XmlSerializerUsage
    {
        XmlSerializerUsage(EndpointConfiguration endpointConfiguration)
        {
            #region XmlSerialization

            endpointConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }
    }
}