using NServiceBus;
using NServiceBus.Logging;

#region CustomCustomizeConfiguration
public class CustomizeConfiguration : ICustomizeConfiguration
{
    static ILog log = LogManager.GetLogger<CustomizeConfiguration>();

    public void Run(BusConfiguration busConfiguration)
    {
        log.Info("Setting serializer to JSON in an extension");
        busConfiguration.UseSerialization<JsonSerializer>();
    }
}
#endregion