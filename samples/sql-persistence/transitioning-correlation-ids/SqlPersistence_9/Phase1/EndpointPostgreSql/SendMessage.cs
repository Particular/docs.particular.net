using System.Threading.Tasks;
using NServiceBus;

partial class Program
{
    static Task SendMessage(IMessageSession messageSession)
    {
        var message = new StartOrder
        {
            OrderNumber = 10
        };
        return messageSession.SendLocal(message);
    }
}