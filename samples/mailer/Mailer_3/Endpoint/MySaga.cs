using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Mailer;

#region saga

public class MySaga :
    Saga<MySagaData>,
    IAmStartedByMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MySaga>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var mail = new Mail
        {
            To = "to@fake.email",
            From = "from@fake.email",
            Body = "This is the body",
            Subject = "Hello from saga",
            AttachmentContext = new Dictionary<string, string>
            {
                {"Id", "fakeEmail"}
            }
        };
        await context.SendMail(mail)
            .ConfigureAwait(false);
        log.Info($"Mail sent and written to {Program.DirectoryLocation}");
        MarkAsComplete();
    }

    #endregion

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.ConfigureMapping<MyMessage>(message => message.Number).ToSaga(data => data.Number);
    }
}