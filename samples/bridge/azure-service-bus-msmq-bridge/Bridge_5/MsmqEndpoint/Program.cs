using System;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

Console.Title = "MsmqEndpoint";

var endpointConfiguration = new EndpointConfiguration("Samples.MessagingBridge.MsmqEndpoint");
endpointConfiguration.EnableInstallers();
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.UsePersistence<NonDurablePersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routingConfig = endpointConfiguration.UseTransport(new MsmqTransport());
routingConfig.RegisterPublisher(typeof(OtherEvent), "Samples.MessagingBridge.AsbEndpoint");

//var defaultFactory = LogManager.Use<DefaultFactory>();
//defaultFactory.Level(LogLevel.Debug);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
