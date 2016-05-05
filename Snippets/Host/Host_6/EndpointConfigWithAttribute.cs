using NServiceBus;

#region EndpointNameByAttribute
[EndpointName("MyEndpointName")]
public class EndpointConfigWithAttribute : IConfigureThisEndpoint, AsA_Server
{
    // ... the config
    #endregion
    public void Customize(BusConfiguration busConfiguration)
    {
    }
}