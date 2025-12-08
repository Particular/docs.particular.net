using System.Text.Json;
using Microsoft.Extensions.Hosting;

Console.Title = "CloudEvents";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.CloudEvents");
endpointConfiguration.EnableInstallers();

#region cloudevents-serialization
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    IncludeFields = true
});
#endregion

#region cloudevents-configuration
endpointConfiguration.EnableCloudEvents().TypeMappings = new Dictionary<string, Type[]>
{
    ["ObjectCreated:Put"] = [typeof(AwsBlobNotification)]
};
#endregion

var transport = new SqsTransport();
endpointConfiguration.UseTransport(transport);


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
