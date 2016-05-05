namespace Core3.EndpointName
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region EndpointNameCode

            // To customize the endpoint name via code use the DefineEndpointName method,
            // it is important to call it first, right after the With() configuration entry point.
            configure.DefineEndpointName("MyEndpoint");

            #endregion
        }
    }
}