using NServiceBus;
using Shared;
using System;
using System.Threading.Tasks;
using NServiceBus.ClaimCheck;
using Microsoft.Extensions.Hosting;


Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");

#region ConfigureReceiverCustomDataBusSerializer

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, BsonClaimCheckSerializer>();
claimCheck.BasePath(@"..\..\..\..\storage");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();