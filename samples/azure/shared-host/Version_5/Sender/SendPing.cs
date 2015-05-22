namespace Sender
{
    using System.Diagnostics;
    using NServiceBus;

    public class SendPing : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Trace.WriteLine("Pinging Receiver");
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