using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluent 5

        var configuration = new BusConfiguration();

        configuration.EndpointName("MyEndpoint");
        
        #endregion
    }

}

// startcode EndpointNameByAttribute 5
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
    public void Customize(BusConfiguration configuration)
    {
    }
}

// startcode EndpointNameByNamespace 5
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
