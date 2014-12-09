
#region customize_nsb_host
using NServiceBus;
class CustomizingHost : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration configuration)
    {
        // To customize, use the configuration parameter. 
        // For example, to customize the endpoint name:
        configuration.EndpointName("NewEndpointName");
    }
}
#endregion
