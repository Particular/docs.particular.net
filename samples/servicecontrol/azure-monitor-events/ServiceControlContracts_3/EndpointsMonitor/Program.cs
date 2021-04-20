using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "AzureMonitorConnector";
        var endpointConfiguration = new EndpointConfiguration("AzureMonitorConnector");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(
            type =>
            {
                return typeof(IEvent).IsAssignableFrom(type) ||
                       // include ServiceControl events
                       type.Namespace != null &&
                       type.Namespace.StartsWith("ServiceControl.Contracts");
            });

        #region AppInsightsSdkSetup

        var envInstrumentationKey = "ApplicationInsightKey";
        var instrumentationKey = Environment.GetEnvironmentVariable(envInstrumentationKey);

        if (string.IsNullOrEmpty(instrumentationKey))
        {
            throw new Exception($"Environment variable '{envInstrumentationKey}' required.");
        }

        Console.WriteLine("Using application insights application key: {0}", instrumentationKey);


        var telemetryConfiguration = new TelemetryConfiguration(instrumentationKey);
        var telemetryClient = new TelemetryClient(telemetryConfiguration);

        endpointConfiguration.RegisterComponents(cc => cc.RegisterSingleton(telemetryClient));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to finish.");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
