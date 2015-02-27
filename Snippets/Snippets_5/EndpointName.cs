using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluent

        BusConfiguration configuration = new BusConfiguration();

        configuration.EndpointName("MyEndpoint");
        
        #endregion
    }

}

// startcode EndpointNameByAttribute
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
    public void Customize(BusConfiguration configuration)
    {
    }
}

// startcode EndpointNameByNamespace
namespace MyServer
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        // ... your custom config
    // endcode
        public void Customize(BusConfiguration co)
        {
        }
    }
}
