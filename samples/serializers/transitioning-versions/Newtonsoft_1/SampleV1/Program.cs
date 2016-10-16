using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Serialization.TransitionV1";

        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.TransitionV1");
        endpointConfiguration.SharedConfig();

        #region configv1

        var settingsV1 = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        var serializationV1 = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serializationV1.Settings(settingsV1);
        serializationV1.ContentTypeKey("jsonv1");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            var message = MessageCreator.NewOrder();
            await endpointInstance.SendLocal(message)
                .ConfigureAwait(false);
            await endpointInstance.Send("Samples.Serialization.TransitionV2", message)
                .ConfigureAwait(false);
            Console.WriteLine("Order Sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}