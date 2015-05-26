namespace Receiver
{
    using NServiceBus;
    using Shared;

    #region AzureMultiHost_PingHandler

    public class PingHandler : IHandleMessages<Ping>
    {
        public IBus Bus { get; set; }

        public void Handle(Ping message)
        {
            VerificationLogger.Write("Receiver", "Got Ping and will reply with Pong");
            Bus.Reply(new Pong());
        }
    }

    #endregion
}