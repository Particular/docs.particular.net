using System.Threading.Tasks;
using NServiceBus;

#pragma warning disable 618

#region AzureMultiHost_SendPingCommand

public class SendPing :
    IWantToRunWhenEndpointStartsAndStops
{
    public Task Start(IMessageSession session)
    {
        VerificationLogger.Write("Sender", "Pinging Receiver");
        return session.Send(new Ping());
    }

    public Task Stop(IMessageSession session)
    {
        return Task.CompletedTask;
    }
}

#endregion

#pragma warning restore 618
