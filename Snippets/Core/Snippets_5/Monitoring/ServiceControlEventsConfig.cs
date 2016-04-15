namespace Core5.Monitoring
{
    using NServiceBus;

    class ServiceControlEventsConfig
    {
        ServiceControlEventsConfig(BusConfiguration busConfiguration)
        {
            #region ServiceControlEventsConfig 

            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.Conventions()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       //include ServiceControl events
                                       t.Namespace != null && 
                                       t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }
    }
}