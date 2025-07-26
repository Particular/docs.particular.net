using System.Threading.Tasks;
using NServiceBus;

#region InjectingMessageSession
public class MessageSender(IMessageSession messageSession)
{
    public Task SendMessage()
    {
        var myMessage = new MyMessage();
        return messageSession.SendLocal(myMessage);
    }
}
#endregion