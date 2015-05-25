namespace Receiver
{
    using NServiceBus;
    using Shared;

    public class PingHandler : IHandleMessages<Ping>
    {
        public IBus Bus { get; set; }

        public void Handle(Ping message)
        {
            Logger.WriteLine("Receiver got Ping and will reply with Pong");
            Bus.Reply(new Pong
            {
                Message = "Pong for Ping with message:" + message.Message
            });    
        }
    }
}