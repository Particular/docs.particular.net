using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Mailer;

class Usage
{
    void Enabling()
    {
        #region MailerEnabling

        var endpointConfiguration = new EndpointConfiguration("NServiceBusMailSample");
        var mailerSettings = endpointConfiguration.EnableMailer();

        #endregion
    }

    void AttachmentFinder(EndpointConfiguration endpointConfiguration)
    {
        #region MailerAttachmentFinder
        var mailerSettings = endpointConfiguration.EnableMailer();
        mailerSettings.UseAttachmentFinder(
            findAttachments: attachmentContext =>
            {
                var id = attachmentContext["Id"];
                var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello"));
                var attachment = new Attachment(memoryStream, "example.txt", "text/plain");
                var attachments = new List<Attachment> { attachment };
                return Task.FromResult<IEnumerable<Attachment>>(attachments);
            },
            cleanAttachments: attachmentContext =>
            {
                // Attachment cleanup can be performed here
                return Task.CompletedTask;
            });

        #endregion
    }
    void SmtpBuilder(EndpointConfiguration endpointConfiguration)
    {
        #region MailerSmtpBuilder
        var mailerSettings = endpointConfiguration.EnableMailer();
        mailerSettings.UseSmtpBuilder(
            buildSmtpClient: () =>
            {
                return new SmtpClient
                {
                    EnableSsl = true,
                    Port = 1000,
                };
            });

        #endregion
    }
    void TestSmtpBuilder(EndpointConfiguration endpointConfiguration)
    {
        #region MailerTestSmtpBuilder

        var mailerSettings = endpointConfiguration.EnableMailer();
        mailerSettings.UseSmtpBuilder(
            buildSmtpClient: () =>
            {
                var directoryLocation = Path.Combine(Environment.CurrentDirectory, "Emails");
                Directory.CreateDirectory(directoryLocation);
                return new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = directoryLocation
                };
            });

        #endregion
    }
}