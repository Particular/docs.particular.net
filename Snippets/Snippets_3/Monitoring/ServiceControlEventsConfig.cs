namespace Snippets3.Monitoring
{
    using NServiceBus;

    public class ServiceControlEventsConfig
    {
        public void Simple()
        {
            #region ServiceControlEventsConfig

            Configure.Instance
                .JsonSerializer()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       //include ServiceControl events
                                       t.Namespace != null && 
                                       t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }

    }
}