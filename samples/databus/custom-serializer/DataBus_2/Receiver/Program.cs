using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;
using Shared;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");

#region ConfigureReceiverCustomDataBusSerializer

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, BsonClaimCheckSerializer>();
claimCheck.BasePath(SolutionDirectoryFinder.Find("storage"));

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();