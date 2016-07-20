namespace Core4.Monitoring
{
    using NServiceBus;

    class ServiceControlEventsConfig
    {
        ServiceControlEventsConfig(Configure configure)
        {
            #region ServiceControlEventsConfig

            Configure.Serialization.Json();
            configure.DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       // include ServiceControl events
                                       t.Namespace != null &&
                                       t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }

    }
}