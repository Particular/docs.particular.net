using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region CustomCustomizeConfiguration
public class CustomizeConfiguration :
    ICustomizeConfiguration
{
    static ILog log = LogManager.GetLogger<CustomizeConfiguration>();
    public Task Run(EndpointConfiguration endpointConfiguration)
    {
        log.Info("Setting serializer to JSON in an extension");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        return Task.CompletedTask;
    }
}
#endregion