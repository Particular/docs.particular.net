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
        Console.Title = "Samples.PipelineStream.Sender";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.PipelineStream.Sender");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region configure-stream-storage

        endpointConfiguration.SetStreamStorageLocation("..\\..\\..\\storage");

        #endregion

        endpointConfiguration.EnableInstallers();
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await Run(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'F' to send a message with a file stream");
        Console.WriteLine("Press 'H' to send a message with a http stream");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.F)
            {
                await SendMessageWithFileStream(endpointInstance);
                continue;
            }
            if (key.Key == ConsoleKey.H)
            {
                await SendMessageWithHttpStream(endpointInstance);
                continue;
            }
            break;
        }
    }

    static async Task SendMessageWithFileStream(IEndpointInstance endpointInstance)
    {
        #region send-message-with-file-stream

        MessageWithStream message = new MessageWithStream
        {
            SomeProperty = "This message contains a stream",
            StreamProperty = File.OpenRead("FileToSend.txt")
        };
        await endpointInstance.Send("Samples.PipelineStream.Receiver", message);

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with file stream sent");
    }

    static async Task SendMessageWithHttpStream(IEndpointInstance endpointInstance)
    {
        #region send-message-with-http-stream

        using (WebClient webClient = new WebClient())
        {
            MessageWithStream message = new MessageWithStream
            {
                SomeProperty = "This message contains a stream",
                StreamProperty = webClient.OpenRead("http://www.particular.net")
            };
            await endpointInstance.Send("Samples.PipelineStream.Receiver", message);
        }

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with http stream sent");
    }
}