using System.Threading.Tasks;
using NServiceBus;

public class MessageSender(IMessageSession messageSession)
{
    public async Task SendMessage()
    {
        var message = new MyMessage();
        await messageSession.SendLocal(message);
    }
}
