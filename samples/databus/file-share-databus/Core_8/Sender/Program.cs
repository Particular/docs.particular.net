using NServiceBus;
using Shared;
using System;
using System.Threading.Tasks;

class Program
{
    static string storagePath = new SolutionDirectoryFinder().GetDirectory("storage");
    
    static async Task Main()
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

        #region ConfigureDataBus

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        dataBus.BasePath(storagePath);

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<XmlSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press 'D' to send a databus large message");
        Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.N)
            {
                await SendMessageTooLargePayload(endpointInstance);
                continue;
            }

            if (key.Key == ConsoleKey.D)
            {
                await SendMessageLargePayload(endpointInstance);
                continue;
            }
            break;
        }
        await endpointInstance.Stop();
    }


    static async Task SendMessageLargePayload(IEndpointInstance endpointInstance)
    {
        #region SendMessageLargePayload

        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new DataBusProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message);

        #endregion

        Console.WriteLine($"Message sent, the payload is stored in: {storagePath}");
    }

    static async Task SendMessageTooLargePayload(IEndpointInstance endpointInstance)
    {
        #region SendMessageTooLargePayload

        var message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024*1024*5] //5MB
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message);

        #endregion
    }
}
