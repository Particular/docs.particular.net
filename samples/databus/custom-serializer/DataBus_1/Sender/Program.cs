using NServiceBus;
using Shared;
using System;
using Microsoft.Extensions.Hosting;
using Sender;
using Microsoft.Extensions.DependencyInjection;


Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#region ConfigureSenderCustomDataBusSerializer

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, BsonClaimCheckSerializer>();
claimCheck.BasePath(@"..\..\..\..\storage");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();