using System.Text.Json;
using Microsoft.Extensions.Hosting;

Console.Title = "CloudEvents";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.CloudEvents");
endpointConfiguration.EnableInstallers();

#region sqs-cloudevents-serialization
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    IncludeFields = true
});
#endregion

#region sqs-cloudevents-configuration
var cloudEventsConfiguration = endpointConfiguration.EnableCloudEvents();
#endregion

#region sqs-cloudevents-typemapping
cloudEventsConfiguration.TypeMappings["ObjectCreated:Put"] = [typeof(AwsBlobNotification)];
#endregion

#region sqs-cloudevents-json-permissive
cloudEvents.EnvelopeUnwrappers.Find<CloudEventJsonStructuredEnvelopeUnwrapper>().EnvelopeHandlingMode = JsonStructureEnvelopeHandlingMode.Permissive;
#endregion

var transport = new SqsTransport();
endpointConfiguration.UseTransport(transport);


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
