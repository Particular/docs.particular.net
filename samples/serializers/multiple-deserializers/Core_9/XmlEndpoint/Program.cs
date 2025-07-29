using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "XmlEndpoint";

#region configXml

var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.XmlEndpoint");
endpointConfiguration.UseSerialization<XmlSerializer>();
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