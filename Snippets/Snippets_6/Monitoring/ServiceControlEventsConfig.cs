namespace Snippets6.Monitoring
{
    using NServiceBus;

    public class ServiceControlEventsConfig
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ServiceControlEventsConfig 

            // required to talk to ServiceControl
            busConfiguration.UseLegacyMessageDrivenSubscriptionMode();
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.Conventions()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       //include ServiceControl events
                                       t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }
    }
}