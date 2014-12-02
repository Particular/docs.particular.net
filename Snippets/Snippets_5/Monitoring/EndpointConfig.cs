namespace MonitoringNotifications.ServiceControl.Notifications
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;

    #region MessageFailedEndpointConfig 5.0
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseSerialization<JsonSerializer>();

            configuration.Conventions()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || IsServiceControlContract(t));

            configuration.UsePersistence<RavenDBPersistence>();
        }

        private static bool IsServiceControlContract(Type t)
        {
            return t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts");
        }
    }
    #endregion
}