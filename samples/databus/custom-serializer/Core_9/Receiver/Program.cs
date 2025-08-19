using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;
using System;
using System.Threading.Tasks;

Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");

#pragma warning disable CS0618 // Type or member is obsolete
#region ConfigureReceiverCustomDataBusSerializer

var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, BsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\storage");

#endregion
#pragma warning restore CS0618 // Type or member is obsolete

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.ReadKey();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();