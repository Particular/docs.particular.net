using NServiceBus;

class PerformanceMonitoring
{

    #region enable-sla-host-attribute

    [EndpointSLA("00:03:00")]
    public class EndpointConfig :
        IConfigureThisEndpoint
    {
        #endregion

        public void Customize(EndpointConfiguration endpointConfiguration)
        {
        }
    }

}