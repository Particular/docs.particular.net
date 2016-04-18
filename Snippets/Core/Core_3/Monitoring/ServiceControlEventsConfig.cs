namespace Core3.Monitoring
{
    using NServiceBus;

    class ServiceControlEventsConfig
    {
        ServiceControlEventsConfig(Configure configure)
        {
            #region ServiceControlEventsConfig

            configure
                .JsonSerializer()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       //include ServiceControl events
                                       t.Namespace != null &&
                                       t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }
    }
}