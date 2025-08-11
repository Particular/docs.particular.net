using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NServiceBus;

Console.Title = "TransitionPhase1";

var builder = Host.CreateApplicationBuilder(args);
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

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();
var messageSession = host.Services.GetRequiredService<IMessageSession>();

var message = MessageCreator.NewOrder();

await messageSession.SendLocal(message);

await messageSession.Send("Samples.Serialization.TransitionPhase2", message);
Console.WriteLine("Order Sent");

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();
