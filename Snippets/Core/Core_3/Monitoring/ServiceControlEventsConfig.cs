namespace Core3.Monitoring
{
    using NServiceBus;

    class ServiceControlEventsConfig
    {
        ServiceControlEventsConfig(Configure configure)
        {
            #region ServiceControlEventsConfig

            var serializer = configure.JsonSerializer();
            serializer.DefiningEventsAs(type =>
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