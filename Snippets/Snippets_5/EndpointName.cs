using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluent

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("MyEndpoint");
        
        #endregion
    }

}

// startcode EndpointNameByAttribute
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
    public void Customize(BusConfiguration busConfiguration)
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
        public void Customize(BusConfiguration busConfiguration)
        {
        }
    }
}
