namespace Sender
{
    using NServiceBus;
    using Shared;

    #region AzureMultiHost_SendPingCommand

    public class SendPing : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            VerificationLogger.Write("Sender", "Pinging Receiver");
            Bus.Send(new Ping());
        }

        public void Stop()
        {
        }
    }

    #endregion
}