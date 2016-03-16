namespace Snippets3.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            Configure configure = Configure.With();
            #region EndpointNameCode

            // If you need to customize the endpoint name via code using the DefineEndpointName method, 
            // it is important to call it first, right after the With() configuration entry point.
            configure.DefineEndpointName("MyEndpoint");

            #endregion
        }
    }
}