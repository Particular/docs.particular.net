using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

static class Program
{
    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Serialization.TransitionPhase4";
        var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.TransitionPhase4");
        endpointConfiguration.SharedConfig();

        #region Phase4

        var settingsV2 = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new ExtendedResolver()
        };
        var serializationV2 = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serializationV2.Settings(settingsV2);
        serializationV2.ContentTypeKey("jsonv2");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var message = MessageCreator.NewOrder();
        await endpointInstance.SendLocal(message)
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