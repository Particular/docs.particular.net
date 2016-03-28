namespace Snippets4.EndpointName
{
    using NServiceBus;

    #region EndpointNameByAttribute

    [EndpointName("MyEndpointName")]
    public class EndpointConfigWithAttribute : IConfigureThisEndpoint, AsA_Server
    {
        // ... custom config

        #endregion
    }
}