using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluent
        
        Configure.With()
            // If you need to customize the endpoint name via code using the DefineEndpointName method, 
            // it is important to call it first, right after the With() configuration entry point.
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
