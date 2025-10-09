using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "RightReceiver";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.RightReceiver");
endpointConfiguration.UsePersistence<LearningPersistence>();

endpointConfiguration.Conventions().DefiningCommandsAs(t => t.Name == "PlaceOrder");
endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

#region alternative-learning-transport
var learningTransportDefinition = new LearningTransport
{
    // Set storage directory and add the character '2' to simulate a different transport.
    StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
};
endpointConfiguration.UseTransport(learningTransportDefinition);
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.EnableInstallers();


builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
