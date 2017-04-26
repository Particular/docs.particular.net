using NServiceBus;

#region EndpointNameByAttribute
[EndpointName("MyEndpointName")]
public class EndpointConfigWithAttribute :
    IConfigureThisEndpoint
{
    // ... the config
    #endregion
    public void Customize(BusConfiguration busConfiguration)
    {
    }
}
