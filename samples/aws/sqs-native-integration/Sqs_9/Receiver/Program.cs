using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

Console.Title = "SimpleReceiver";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.SimpleReceiver");
endpointConfiguration.EnableInstallers();

var transport = new SqsTransport
{
    DoNotWrapOutgoingMessages = true
};

#region SerializerConfig
var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serialization.Settings(new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto
});
#endregion

endpointConfiguration.UseTransport(transport);

#region RegisterBehaviorInPipeline

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<AccessToAmazonSqsNativeMessageBehavior>>();
endpointConfiguration.Pipeline.Register(new AccessToAmazonSqsNativeMessageBehavior(logger), "Demonstrates how to access the native message from a pipeline behavior");
#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
