using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameFluentV5

        Configure.With(b => b.EndpointName("MyEndpoint"));

        #endregion
    }

}

// startcode EndpointNameByAttributeV5
[EndpointName("MyEndpointName")]
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    // ... your custom config
// endcode
    public void Customize(ConfigurationBuilder builder)
    {
    }
}

// startcode EndpointNameByNamespaceV5
namespace MyServer
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        // ... your custom config
    // endcode
        public void Customize(ConfigurationBuilder builder)
        {
        }
    }
}
