using Microsoft.Extensions.Hosting;
using NServiceBus;

#region asb-function-isolated-configuration
[assembly: NServiceBusTriggerFunction("WorkerDemoEndpoint")]

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
#endregion asb-function-isolated-configuration

class EnableDiagnostics
{
    #region asb-function-isolated-enable-diagnostics
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(configuration =>
            {
                configuration.LogDiagnostics();
            })
            .Build();

        host.Run();
    }
    #endregion
}

class ConfigureErrorQueue
{
    #region asb-function-isolated-configure-error-queue
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(configuration =>
            {
                // Change the error queue name:
                configuration.AdvancedConfiguration.SendFailedMessagesTo("my-custom-error-queue");

                // Or disable the error queue to let ASB native dead-lettering handle repeated failures:
                configuration.DoNotSendMessagesToErrorQueue();
            })
            .Build();

        host.Run();
    }
    #endregion
}