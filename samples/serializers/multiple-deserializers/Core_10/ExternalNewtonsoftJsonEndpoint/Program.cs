using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "NewtonsoftJsonEndpoint";

#region configExternalNewtonsoftJson

var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ExternalNewtonsoftJsonEndpoint");
var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serialization.ContentTypeKey("NewtonsoftJson");
endpointConfiguration.RegisterOutgoingMessageLogger();

#endregion
endpointConfiguration.UseTransport(new LearningTransport());

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var message = MessageBuilder.BuildMessage();

await messageSession.Send("Samples.MultipleDeserializers.ReceivingEndpoint", message);

Console.WriteLine("Order Sent");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();