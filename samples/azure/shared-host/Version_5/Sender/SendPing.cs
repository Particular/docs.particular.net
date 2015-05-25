namespace Sender
{
    using NServiceBus;
    using Shared;

    public class SendPing : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Logger.WriteLine("Sender is pinging Receiver");
            Bus.Send(new Ping
            {
                Message = "Originated at Sender"
            });
        }

        public void Stop()
        {
            
        }
    }
}