using System.Threading.Tasks;
using NServiceBus;

public class Startup :
    IWantToRunWhenEndpointStartsAndStops
{
    public Task Start(IMessageSession session)
    {
        var myMessage = new MyMessage();
        return session.SendLocal(myMessage);
    }

    public Task Stop(IMessageSession session)
    {
        return Task.CompletedTask;
    }
}