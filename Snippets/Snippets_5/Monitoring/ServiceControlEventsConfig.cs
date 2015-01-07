using NServiceBus;

public class ServiceControlEventsConfig
{
    public void Simple()
    {
        var configuration = new BusConfiguration();

        #region ServiceControlEventsConfig 5.0

        configuration.UseSerialization<JsonSerializer>();
        configuration.Conventions()
            .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) ||
                                   //include ServiceControl events
                                   t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"));

        #endregion
    }
}