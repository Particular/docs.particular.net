using System.Threading.Tasks;
using NServiceBus;

#region AzureMultiHost_SendPingCommand

public class SendPing : IWantToRunWhenEndpointStartsAndStops
{
    public async Task Start(IMessageSession session)
    {
        VerificationLogger.Write("Sender", "Pinging Receiver");
        await session.Send(new Ping());
    }

    public Task Stop(IMessageSession session)
    {
        return Task.FromResult(0);
    }
}

#endregion
