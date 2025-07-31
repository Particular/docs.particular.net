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
claimCheck.BasePath(@"..\..\..\..\storage");

#endregion

#region ClaimCheckConventions
endpointConfiguration.Conventions().DefiningClaimCheckPropertiesAs(prop => prop.Name.StartsWith("Large"));
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.ReadKey();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();