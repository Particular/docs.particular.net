using NServiceBus;

#region AzureMultiHost_PongHandler

public class PongHandler : IHandleMessages<Pong>
{
    public void Handle(Pong message)
    {
        VerificationLogger.Write("Sender", "Got Pong from Receiver");
    }
}

#endregion
