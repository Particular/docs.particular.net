namespace Core6.Monitoring
{
    using NServiceBus;

    class ServiceControlEventsConfig
    {
        ServiceControlEventsConfig(EndpointConfiguration endpointConfiguration)
        {
            #region ServiceControlEventsConfig 

            // required to talk to ServiceControl
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.Conventions()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       // include ServiceControl events
                                       t.Namespace != null &&
                                       t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }
    }
}