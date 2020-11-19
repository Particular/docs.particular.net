using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

#region configuration-with-function-host-builder

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        // register custom service in the container
        services.AddSingleton(_ =>
        {
            var configurationRoot = builder.GetContext().Configuration;
            var customComponentInitializationValue = configurationRoot.GetValue<string>("CustomComponentValue");

            return new CustomComponent(customComponentInitializationValue);
        });

        builder.UseNServiceBus(() =>
        {
            var configuration = new StorageQueueTriggeredEndpointConfiguration(AzureStorageQueueTriggerFunction.EndpointName);

            configuration.UseSerialization<NewtonsoftSerializer>();
            
            // Disable persistence requirement
            configuration.Transport.DisablePublishing();

            // optional: log startup diagnostics using Functions provided logger
            configuration.LogDiagnostics();

            return configuration;
        });
    }
}

#endregion