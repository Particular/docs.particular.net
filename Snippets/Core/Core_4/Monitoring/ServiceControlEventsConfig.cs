namespace Core4.Monitoring
{
    using NServiceBus;

    class ServiceControlEventsConfig
    {
        ServiceControlEventsConfig(Configure configure)
        {
            #region ServiceControlEventsConfig

            Configure.Serialization.Json();
            configure.DefiningEventsAs(type =>
            {
                return typeof(IEvent).IsAssignableFrom(type) ||
                       // include ServiceControl events
                       type.Namespace != null &&
                       type.Namespace.StartsWith("ServiceControl.Contracts");
            });

            #endregion
        }

    }
}