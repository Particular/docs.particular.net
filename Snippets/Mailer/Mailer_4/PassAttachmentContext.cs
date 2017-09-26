using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Mailer;

public class PassAttachmentContext :
    IHandleMessages<MyMessage>
{
    #region MailerPassAttachmentContext
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var mail = new Mail
        {
            To = "to@fake.email",
            From = "from@fake.email",
            Body = "This is the body",
            Subject = "Hello",
            AttachmentContext = new Dictionary<string, string>
            {
                { "Id", "fakeEmail" }
            }
        };
        return context.SendMail(mail);
    }
    #endregion
}

