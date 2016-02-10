namespace Snippets6.Monitoring
{
    using NServiceBus;

    public class ServiceControlEventsConfig
    {
        public void Simple()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region ServiceControlEventsConfig 

            // required to talk to ServiceControl
            configuration.UseLegacyMessageDrivenSubscriptionMode();
            configuration.UseSerialization<JsonSerializer>();
            configuration.Conventions()
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                       //include ServiceControl events
                                       t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"));

            #endregion
        }
    }
}