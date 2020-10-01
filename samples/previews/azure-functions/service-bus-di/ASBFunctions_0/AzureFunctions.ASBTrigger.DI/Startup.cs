using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

#region EndpointSetup

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton(sp => new FunctionEndpoint(executionContext =>
        {
            // endpoint name, and connection strings are automatically derived from FunctionName and ServiceBusTrigger attributes
            var configuration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

            configuration.UseSerialization<NewtonsoftSerializer>();

            // optional: log startup diagnostics using Functions provided logger
            configuration.LogDiagnostics();

            /*//resolve telemetry configuration
            var telemetryConfiguration = sp.GetRequiredService<TelemetryConfiguration>();

            //register in the endpoints container
            configuration.AdvancedConfiguration.RegisterComponents(cc => cc.RegisterSingleton(telemetryConfiguration));*/

            return configuration;
        }));
    }
}

#endregion