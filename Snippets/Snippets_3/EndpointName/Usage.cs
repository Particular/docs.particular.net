namespace Snippets3.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EndpointNameCode

            Configure.With()
                // If you need to customize the endpoint name via code using the DefineEndpointName method, 
                // it is important to call it first, right after the With() configuration entry point.
                .DefineEndpointName("MyEndpoint");

            #endregion
        }
    }
}