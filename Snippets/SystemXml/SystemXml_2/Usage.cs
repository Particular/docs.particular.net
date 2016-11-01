using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SystemXmlSerialization

        endpointConfiguration.UseSerialization<SystemXmlSerializer>();

        #endregion
    }

}