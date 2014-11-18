using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluentV4
        
        Configure.With()
            .DefineEndpointName("MyEndpoint");

        #endregion
    }
}

// startcode EndpointNameByAttributeV4
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
}

// startcode EndpointNameByNamespaceV4
namespace MyServer
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        // ... your custom config
        // endcode
    }
}
