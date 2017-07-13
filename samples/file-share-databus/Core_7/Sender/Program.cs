using System;
using System.Threading.Tasks;
using NServiceBus;

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

        #region ConfigureDataBus

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath("..\\..\\..\\storage");

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press 'D' to send a databus large message");
        Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.N)
            {
                await SendMessageTooLargePayload(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }

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

        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new DataBusProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Message sent, the payload is stored in: ..\\..\\..\\storage");
    }

    static async Task SendMessageTooLargePayload(IEndpointInstance endpointInstance)
    {
        #region SendMessageTooLargePayload

        var message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024*1024*5] //5MB
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message)
            .ConfigureAwait(false);

        #endregion
    }
}