using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Spring

        endpointConfiguration.UseContainer<SimpleInjectorBuilder>();

        #endregion
    }
}