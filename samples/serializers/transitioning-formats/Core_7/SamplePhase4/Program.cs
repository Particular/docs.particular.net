﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "TransitionPhase4";
        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.TransitionPhase4");
        endpointConfiguration.SharedConfig();

        #region Phase4

        var settingsV2 = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new ExtendedResolver()
        };
        var serializationV2 = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serializationV2.Settings(settingsV2);
        serializationV2.ContentTypeKey("jsonv2");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var message = MessageCreator.NewOrder();
        await endpointInstance.SendLocal(message);
        await endpointInstance.Send("Samples.Serialization.TransitionPhase3", message);
        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}