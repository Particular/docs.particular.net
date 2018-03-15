using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PipelineStream.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.PipelineStream.Sender");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region configure-stream-storage

        endpointConfiguration.SetStreamStorageLocation(@"..\..\..\..\storage");

        #endregion

        endpointConfiguration.EnableInstallers();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }


    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'F' to send a message with a file stream");
        Console.WriteLine("Press 'H' to send a message with an http stream");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.F)
            {
                await SendMessageWithFileStream(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            if (key.Key == ConsoleKey.H)
            {
                await SendMessageWithHttpStream(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            break;
        }
    }

    static async Task SendMessageWithFileStream(IEndpointInstance endpointInstance)
    {
        #region send-message-with-file-stream

        var message = new MessageWithStream
        {
            SomeProperty = "This message contains a stream",
            StreamProperty = File.OpenRead("FileToSend.txt")
        };
        await endpointInstance.Send("Samples.PipelineStream.Receiver", message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with file stream sent");
    }

    static async Task SendMessageWithHttpStream(IEndpointInstance endpointInstance)
    {
        #region send-message-with-http-stream

        using (var webClient = new WebClient())
        {
            var message = new MessageWithStream
            {
                SomeProperty = "This message contains a stream",
                StreamProperty = webClient.OpenRead("http://www.particular.net")
            };
            await endpointInstance.Send("Samples.PipelineStream.Receiver", message)
                .ConfigureAwait(false);
        }

        #endregion

        Console.WriteLine();
        Console.WriteLine("Message with http stream sent");
    }
}