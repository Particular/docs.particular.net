using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

#region configuration-with-function-host-builder

[assembly: FunctionsStartup(typeof(Startup))]
[assembly: NServiceBusTriggerFunction(Startup.EndpointName)]

public class Startup : FunctionsStartup
{
    public const string EndpointName = "ASBTriggerQueue";

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

        builder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration(EndpointName));
    }
}

#endregion