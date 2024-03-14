using System;
using Newtonsoft.Json;
using NServiceBus;

Console.Title = "Samples.Serialization.TransitionPhase1";

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.TransitionPhase1");
endpointConfiguration.SharedConfig();

#region Phase1

var settingsV1 = new JsonSerializerSettings
{
    Formatting = Formatting.Indented
};

var serializationV1 = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serializationV1.Settings(settingsV1);
serializationV1.ContentTypeKey("jsonv1");

#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);

var message = MessageCreator.NewOrder();

await endpointInstance.SendLocal(message);

await endpointInstance.Send("Samples.Serialization.TransitionPhase2", message);

Console.WriteLine("Order Sent");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
