using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Settings;
using NServiceBus.TransportTests;

public class ConfigureFileTransportInfrastructure : IConfigureTransportInfrastructure
{
    public TransportConfigurationResult Configure(SettingsHolder settings, TransportTransactionMode transactionMode)
    {
        return new TransportConfigurationResult
        {
            PurgeInputQueueOnStartup = true,
            TransportInfrastructure = new FileTransportInfrastructure()
        };
    }

    public Task Cleanup()
    {
        return Task.CompletedTask;
    }
}