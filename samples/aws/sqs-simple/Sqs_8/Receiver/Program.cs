using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "SimpleReceiver";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.SimpleReceiver");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var transport = new SqsTransport
{
    S3 = new S3Settings("bucketname", "my/key/prefix")
};
endpointConfiguration.UseTransport(transport);

Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
