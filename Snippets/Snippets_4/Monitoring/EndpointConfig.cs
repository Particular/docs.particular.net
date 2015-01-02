namespace MonitoringNotifications.ServiceControl.Notifications
{
    using System;
    using NServiceBus;
    using NServiceBus.Features;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public EndpointConfig()
        {
            Configure.Serialization.Json();
        }
    }

    internal class UnobtrusiveMessageConventions : IWantToRunBeforeConfiguration
    {
        public void Init()
        {
        	
	#region MessageFailedEndpointConfig        	
            Configure.Instance
                .DefiningEventsAs(t => typeof (IEvent).IsAssignableFrom(t) || IsServiceControlContract(t));
        }

        private static bool IsServiceControlContract(Type t)
        {
            return t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts");
        }
        
        #endregion
    }
}
