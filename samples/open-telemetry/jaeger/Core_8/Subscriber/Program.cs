using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string EndpointName = "Samples.OpenTelemetry.Subscriber";

Console.Title = EndpointName;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
    .AddSource("NServiceBus.Core")
    .AddJaegerExporter()
    .Build();

var endpointConfiguration = new EndpointConfiguration(EndpointName);

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
_ = Console.ReadKey();

await endpointInstance.Stop();
tracerProvider.ForceFlush();