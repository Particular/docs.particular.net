using System;
using Newtonsoft.Json;
using NServiceBus;

Console.Title = "Samples.Sqs.SimpleReceiver";

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
endpointConfiguration.Pipeline.Register(new AccessToAmazonSqsNativeMessageBehavior(), "Demonstrates how to access the native message from a pipeline behavior");
#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();