using NServiceBus;
using Shared;
using System;
using Microsoft.Extensions.Hosting;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.ClaimCheck.Receiver");
var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
var storagePath = new SolutionDirectoryFinder().GetDirectory("storage");
claimCheck.BasePath(storagePath);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();

await host.StartAsync();

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();

