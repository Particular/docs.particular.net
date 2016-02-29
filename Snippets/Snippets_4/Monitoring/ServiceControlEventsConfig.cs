namespace Snippets4.Monitoring
{
    using NServiceBus;

    public class ServiceControlEventsConfig
    {
        public void Simple()
        {
            #region ServiceControlEventsConfig

            Configure.Serialization.Json();
            Configure.Instance
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       //include ServiceControl events
                                       t.Namespace != null && 
                                       t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }

    }
}