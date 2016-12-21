using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Mailer;

#region MailerHandlerUsage
public class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var mail = new Mail
        {
            To = "to@fake.email",
            From = "from@fake.email",
            Body = "This is the body",
            Subject = "Hello"
        };
        return context.SendMail(mail);
    }
}
#endregion