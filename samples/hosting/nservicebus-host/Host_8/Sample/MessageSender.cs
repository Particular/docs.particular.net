using System.Threading.Tasks;
using NServiceBus;

#pragma warning disable 618
#region RunWhenStartsAndStops

public class MessageSender :
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

#endregion
#pragma warning restore 618
