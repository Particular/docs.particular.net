using System.ComponentModel.Composition;
using NServiceBus;
using NServiceBus.Logging;

#region MefCustomizeConfiguration
[Export(typeof(ICustomizeConfiguration))]
public class CustomizeConfiguration :
    ICustomizeConfiguration
{
    static ILog log = LogManager.GetLogger<CustomizeConfiguration>();

    public void Run(BusConfiguration busConfiguration)
    {
        log.Info("Setting serializer to JSON in an extension");
        busConfiguration.UseSerialization<JsonSerializer>();
    }
}
#endregion