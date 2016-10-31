using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region simpleinjector

        endpointConfiguration.UseContainer<SimpleInjectorBuilder>();

        #endregion
    }
}