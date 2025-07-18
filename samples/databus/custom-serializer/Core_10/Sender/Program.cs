using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#pragma warning disable CS0618 // Type or member is obsolete
#region ConfigureSenderCustomDataBusSerializer

var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, BsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\storage");

#endregion
#pragma warning restore CS0618 // Type or member is obsolete

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();