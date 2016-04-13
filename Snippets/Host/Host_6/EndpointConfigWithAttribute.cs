namespace Snippets5.EndpointName
{
    using NServiceBus;

    #region EndpointNameByAttribute
    [EndpointName("MyEndpointName")]
    public class EndpointConfigWithAttribute : IConfigureThisEndpoint, AsA_Server
    {
        // ... your config
        #endregion
        public void Customize(BusConfiguration busConfiguration)
        {
        }
    }

}
