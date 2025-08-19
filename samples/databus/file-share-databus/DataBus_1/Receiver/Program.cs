using System;
using Shared;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Receiver";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ClaimCheck.Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
var storagePath = new SolutionDirectoryFinder().GetDirectory("storage");
claimCheck.BasePath(storagePath);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();