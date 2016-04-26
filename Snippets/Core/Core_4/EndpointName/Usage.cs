namespace Core4.EndpointName
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region EndpointNameCode

            // To customize the endpoint name via code using the DefineEndpointName method,
            // it is important to call it first, right after the With() configuration entry point.
            configure.DefineEndpointName("MyEndpoint");

            #endregion
        }
    }
}


