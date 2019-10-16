using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Attachments.FileShare;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Attachments.FileShare";
        var endpointConfiguration = new EndpointConfiguration("Samples.Attachments.FileShare");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region Enable

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableAttachments(
            fileShare: @"..\..\..\..\storage",
            timeToKeep: TimeToKeep.Default);

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to send a message with an attachment");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            Console.WriteLine();
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                #region send
                var sendOptions = new SendOptions();
                sendOptions.RouteToThisEndpoint();
                var attachments = sendOptions.Attachments();
                attachments.Add(
                    name: "attachmentName",
                    streamFactory: () =>
                    {
                        return File.OpenRead("fileToStream.txt");
                    });

                await endpoint.Send(new MyMessage(), sendOptions)
                    .ConfigureAwait(false);
                #endregion
                continue;
            }
            break;
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}