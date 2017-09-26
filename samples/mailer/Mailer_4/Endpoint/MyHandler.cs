using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Mailer;

#region handler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var mail = new Mail
        {
            To = "to@fake.email",
            From = "from@fake.email",
            Body = "This is the body",
            Subject = "Hello from handler",
            AttachmentContext = new Dictionary<string, string>
            {
                {"Id", "fakeEmail"}
            }
        };
        await context.SendMail(mail)
            .ConfigureAwait(false);
        log.Info($"Mail sent and written to {Program.DirectoryLocation}");
    }
}

#endregion