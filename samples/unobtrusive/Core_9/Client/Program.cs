Console.Title = "Client";
var endpointConfiguration = new EndpointConfiguration("Samples.Unobtrusive.Client");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
#pragma warning disable CS0618 // Type or member is obsolete
var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\DataBusShare\");
#pragma warning restore CS0618 // Type or member is obsolete

endpointConfiguration.ApplyCustomConventions();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
await CommandSender.Start(endpointInstance);
await endpointInstance.Stop();