using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DataBus.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        dataBus.BasePath(@"..\..\..\..\storage");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        //CustomJsonSerializerOptions
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new DatabusPropertyConverterFactory());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);

        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
