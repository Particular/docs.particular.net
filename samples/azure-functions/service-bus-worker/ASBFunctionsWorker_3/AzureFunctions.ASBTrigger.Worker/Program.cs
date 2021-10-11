using Microsoft.Extensions.Hosting;
using NServiceBus;

#region configuration-with-function-host-builder
[assembly:NServiceBusTriggerFunction("ASBWorkerEndpoint")]

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus()
            .Build();

        host.Run();
    }
}
#endregion