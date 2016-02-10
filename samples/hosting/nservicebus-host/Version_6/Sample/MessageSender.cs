using System.Threading.Tasks;
using NServiceBus;

public class MessageSender : IWantToRunWhenBusStartsAndStops
{
    public async Task Start(IMessageSession session)
    {
       await session.SendLocal(new MyMessage());
    }
    public  Task Stop(IMessageSession session)
    {
        return Task.FromResult(0);
    }
}