using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluent 4
        
        Configure.With()
            .DefineEndpointName("MyEndpoint");

        #endregion
    }
}

// startcode EndpointNameByAttribute 4
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
}

// startcode EndpointNameByNamespace 4
namespace MyServer
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        // ... your custom config
        // endcode
    }
}
