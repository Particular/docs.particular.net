#region EndpointNameByNamespace
namespace MyServer
{
    using NServiceBus;

#pragma warning disable 618
    public class EndpointConfigByNamespace :
        IConfigureThisEndpoint
    {
        // ... custom config
        #endregion
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
        }
    }
#pragma warning restore 618
}