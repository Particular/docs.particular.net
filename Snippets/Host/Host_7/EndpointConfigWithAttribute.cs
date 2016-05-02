using NServiceBus;

#region EndpointNameByAttribute

[EndpointName("MyEndpointName")]
public class EndpointConfigWithAttribute : IConfigureThisEndpoint, AsA_Server
{
    // ... custom config

    #endregion

    public void Customize(EndpointConfiguration endpointConfiguration)
    {
    }
}