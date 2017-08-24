using NServiceBus;

#region EndpointNameByAttribute

[EndpointName("MyEndpointName")]
public class EndpointConfigWithAttribute :
    IConfigureThisEndpoint
{
    // ... custom config

    #endregion

    public void Customize(EndpointConfiguration endpointConfiguration)
    {
    }
}
