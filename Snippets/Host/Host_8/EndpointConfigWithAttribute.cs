using NServiceBus;


#pragma warning disable 618
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
#pragma warning restore 618
