Console.Title = "Receiver";

#region ConfigureRabbitQueueName
var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.NativeIntegration");
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString("host=localhost");
#endregion

endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();
