using System.Threading.Tasks;
using NServiceBus;

#region InjectingMessageSession
public class MessageSender
{
    private readonly IMessageSession messageSession;

    public MessageSender(IMessageSession messageSession) =>
        this.messageSession = messageSession;

    public Task SendMessage()
    {
        var myMessage = new MyMessage();
        return messageSession.SendLocal(myMessage);
    }
}
#endregion