using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Mailer;

class Program
{
    public static string DirectoryLocation = Path.Combine(Environment.CurrentDirectory, "Emails");

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Mailer";
        var endpointConfiguration = new EndpointConfiguration("Samples.Mailer");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region EnableMailer

        var mailer = endpointConfiguration.EnableMailer();

        #endregion

        #region smtpBuilder

        mailer.UseSmtpBuilder(
            buildSmtpClient: () =>
            {
                Directory.CreateDirectory(DirectoryLocation);
                return new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = DirectoryLocation
                };
            });

        #endregion

        #region attachmentfinder

        mailer.UseAttachmentFinder(
            findAttachments: attachmentContext =>
            {
                var id = attachmentContext["Id"];
                var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
                var attachment = new Attachment(memoryStream, "example.txt", "text/plain");
                var attachments = new List<Attachment>
                {
                    attachment
                };
                return Task.FromResult<IEnumerable<Attachment>>(attachments);
            },
            cleanAttachments: attachmentContext =>
            {
                // Attachment cleanup can be performed here
                return Task.FromResult(0);
            });

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to send a message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var message = new MyMessage
            {
                Number = Guid.NewGuid()
            };
            await endpoint.SendLocal(message)
                .ConfigureAwait(false);
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}