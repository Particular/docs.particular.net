namespace Sender
{
    using NServiceBus;
    using Shared;

    public class SendPing : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Logger.WriteLine("Sender", "Pinging Receiver");
            Bus.Send(new Ping());
        }

        public void Stop()
        {
            
        }
    }
}