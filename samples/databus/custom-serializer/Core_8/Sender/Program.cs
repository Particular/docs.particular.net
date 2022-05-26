using NServiceBus;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DataBus.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

        #region ConfigureSenderCustomDataBusSerializer

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, BsonDataBusSerializer>();
        dataBus.BasePath(@"..\..\..\..\storage");

        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press Enter to send a databus large message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey(true);
            Console.WriteLine();

            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessageLargePayload(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            break;
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static Task SendMessageLargePayload(IEndpointInstance endpointInstance)
    {
        var measurements = GetMeasurements().ToArray();

        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large collection that will be sent on the data bus",
            LargeData = new DataBusProperty<Measurement[]>(measurements)
        };
        Console.WriteLine(@"Message send, the payload is stored in: ..\..\..\storage");
        return endpointInstance.Send("Samples.DataBus.Receiver", message);
    }

    static IEnumerable<Measurement> GetMeasurements()
    {
        for (var i = 0; i < 10000; i++)
        {
            yield return new Measurement
            {
                Timestamp = DateTimeOffset.UtcNow,
                MeasurementName = $"Instrument {i}",
                MeasurementValue = i * 10m
            };
        }
    }
}