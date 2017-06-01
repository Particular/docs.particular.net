using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.DataBus.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");
        endpointConfiguration.UseSerialization<JsonSerializer>();

        #region ConfigureDataBus

        endpointConfiguration.RegisterComponents(
            cc => cc.ConfigureComponent<JsonDataBusSerializer>(DependencyLifecycle.SingleInstance)
        );


        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath("..\\..\\..\\storage");

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press 'D' to send a databus large message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.D)
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


    static async Task SendMessageLargePayload(IEndpointInstance endpointInstance)
    {
        #region SendMessageLargePayload

        var measurements = GetMeasurements().ToArray();

        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large collection that will be sent on the data bus",
            LargeData = new DataBusProperty<Measurement[]>(measurements)
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Message sent, the payload is stored in: ..\\..\\..\\storage");
    }

    private static IEnumerable<Measurement> GetMeasurements()
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