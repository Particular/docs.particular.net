namespace MonitoringNotifications.ServiceControl.Notifications
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;


    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
    #region MessageFailedEndpointConfig 5.0
            configuration.UseSerialization<JsonSerializer>();

            configuration.Conventions()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || IsServiceControlContract(t));

            .....
        }

        private static bool IsServiceControlContract(Type t)
        {
            return t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts");
        }
    }
    #endregion
}
