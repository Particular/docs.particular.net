using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluent
        
        Configure.With()
            .DefineEndpointName("MyEndpoint");

        #endregion
    }
}

// startcode EndpointNameByAttribute
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
}

// startcode EndpointNameByNamespace
namespace MyServer
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        // ... your custom config
        // endcode
    }
}
