using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

#region configuration-with-function-host-builder
[assembly:NServiceBusTriggerFunction("ASBWorkerEndpoint")]

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(c =>
            {
                var config = c.AdvancedConfiguration;
                config.GetSettings().Set("ASBDelayQueue", "delay"); //Make sure that this queue redirects to the input queue for this function, ASBWorkerEndpoint https://docs.microsoft.com/en-us/azure/service-bus-messaging/enable-auto-forward
                config.Pipeline.Register(typeof(RerouteDelayedMessagesBehavior), "Reroutes timeouts via an intermediate queue");
            })
            .Build();

        host.Run();
    }
}
#endregion