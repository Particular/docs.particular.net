using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;


Console.Title = "Receiver";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
#pragma warning disable CS0618 // Type or member is obsolete
var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\storage");
#pragma warning restore CS0618 // Type or member is obsolete

//CustomJsonSerializerOptions
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.Converters.Add(new DatabusPropertyConverterFactory());
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);

endpointConfiguration.UseTransport(new LearningTransport());
Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
