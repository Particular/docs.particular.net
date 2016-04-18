using System.Threading.Tasks;
using NServiceBus;

#region RunWhenStartsAndStops
public class MessageSender : IWantToRunWhenEndpointStartsAndStops
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
#endregion