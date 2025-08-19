using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NServiceBus;

Console.Title = "TransitionPhase2";

var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.TransitionPhase2");
endpointConfiguration.SharedConfig();

#region Phase2

var settingsV1 = new JsonSerializerSettings
{
    Formatting = Formatting.Indented
};

var serializationV1 = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serializationV1.Settings(settingsV1);
serializationV1.ContentTypeKey("jsonv1");

var settingsV2 = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ContractResolver = new ExtendedResolver()
};

var serializationV2 = endpointConfiguration.AddDeserializer<NewtonsoftJsonSerializer>();
serializationV2.Settings(settingsV2);
serializationV2.ContentTypeKey("jsonv2");

#endregion

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();
var messageSession = host.Services.GetRequiredService<IMessageSession>();

var message = MessageCreator.NewOrder();

await messageSession.SendLocal(message);

await messageSession.Send("Samples.Serialization.TransitionPhase1", message);

await messageSession.Send("Samples.Serialization.TransitionPhase3", message);

Console.WriteLine("Order Sent");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();
