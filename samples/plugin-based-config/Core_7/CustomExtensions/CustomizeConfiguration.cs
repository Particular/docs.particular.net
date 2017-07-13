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
        log.Info("Setting serializer to XML in an extension");
        endpointConfiguration.UseSerialization<XmlSerializer>();
        return Task.CompletedTask;
    }
}
#endregion