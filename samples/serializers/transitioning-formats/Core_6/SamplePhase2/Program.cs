using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Serialization.TransitionPhase2";

        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.TransitionPhase2");
        endpointConfiguration.SharedConfig();

        #region Phase2

        var settingsV1 = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        var serializationV1 = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serializationV1.Settings(settingsV1);
        serializationV1.ContentTypeKey("jsonv1");

        var settingsV2 = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new ExtendedResolver()
        };
        var serializationV2 = endpointConfiguration.AddDeserializer<NewtonsoftSerializer>();
        serializationV2.Settings(settingsV2);
        serializationV2.ContentTypeKey("jsonv2");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var message = MessageCreator.NewOrder();
        await endpointInstance.SendLocal(message)
            .ConfigureAwait(false);
        await endpointInstance.Send("Samples.Serialization.TransitionPhase1", message)
            .ConfigureAwait(false);
        await endpointInstance.Send("Samples.Serialization.TransitionPhase3", message)
            .ConfigureAwait(false);

        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

}