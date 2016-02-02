using System;
using System.IO;
using System.Net;
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
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PipelineStream.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        #region configure-stream-storage

        busConfiguration.SetStreamStorageLocation("..\\..\\..\\storage");

        #endregion

        busConfiguration.EnableInstallers();
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            await Run(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static async Task Run(IBusSession busSession)
    {
        Console.WriteLine("Press 'F' to send a message with a file stream");
        Console.WriteLine("Press 'H' to send a message with a http stream");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.F)
            {
                await SendMessageWithFileStream(busSession);
                continue;
            }
            if (key.Key == ConsoleKey.H)
            {
                await SendMessageWithHttpStream(busSession);
                continue;
            }
            break;
        }
    }

    static async Task SendMessageWithFileStream(IBusSession busSession)
    {
        #region send-message-with-file-stream

        MessageWithStream message = new MessageWithStream
        {
            SomeProperty = "This message contains a stream",
            StreamProperty = File.OpenRead("FileToSend.txt")
        };
        await busSession.Send("Samples.PipelineStream.Receiver", message);

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with file stream sent");
    }

    static async Task SendMessageWithHttpStream(IBusSession busSession)
    {
        #region send-message-with-http-stream

        using (WebClient webClient = new WebClient())
        {
            MessageWithStream message = new MessageWithStream
            {
                SomeProperty = "This message contains a stream",
                StreamProperty = webClient.OpenRead("http://www.particular.net")
            };
            await busSession.Send("Samples.PipelineStream.Receiver", message);
        }

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with http stream sent");
    }
}