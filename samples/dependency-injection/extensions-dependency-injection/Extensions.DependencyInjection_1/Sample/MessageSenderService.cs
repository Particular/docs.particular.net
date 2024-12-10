using System.Threading.Tasks;
using NServiceBus;

#region InjectingMessageSession
public class MessageSenderService
{
    private readonly IMessageSession messageSession;

    public MessageSenderService(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public Task SendMessage()
    {
        var myMessage = new MyMessage();
        return messageSession.SendLocal(myMessage);
    }
}
#endregion