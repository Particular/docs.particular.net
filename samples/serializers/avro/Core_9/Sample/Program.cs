using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Avro";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Avro");

endpointConfiguration.UseSerialization<AvroSerializer>();

#endregion

endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.Pipeline.Register(typeof(MessageBodyLogger), "Logs the message body received");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();