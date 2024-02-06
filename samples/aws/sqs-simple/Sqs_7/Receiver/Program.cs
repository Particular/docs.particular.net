using System;
using NServiceBus;

Console.Title = "Samples.Sqs.SimpleReceiver";
var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.SimpleReceiver");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var transport = new SqsTransport
{
    S3 = new S3Settings("bucketname", "my/key/prefix")
};
endpointConfiguration.UseTransport(transport);

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();