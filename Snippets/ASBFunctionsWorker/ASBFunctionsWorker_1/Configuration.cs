using Microsoft.Extensions.Hosting;
using NServiceBus;

#region configuration
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
#endregion configuration

class EnableDiagnostics
{
    #region enable-diagnostics
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
    #region configure-error-queue
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