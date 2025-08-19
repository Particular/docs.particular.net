using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;


Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#pragma warning disable CS0618 // Type or member is obsolete
#region ConfigureDataBus

var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\storage");

#endregion
#pragma warning restore CS0618 // Type or member is obsolete

#region CustomJsonSerializerOptions
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.Converters.Add(new DatabusPropertyConverterFactory());
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);
#endregion

endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
