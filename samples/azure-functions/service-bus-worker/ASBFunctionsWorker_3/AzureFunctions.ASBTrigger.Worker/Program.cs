using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;

#region configuration-with-function-host-builder
[assembly:NServiceBusTriggerFunction("ASBWorkerEndpoint")]

public class Program
{
    public static Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus()
            .Build();

        return host.RunAsync();
    }
}
#endregion