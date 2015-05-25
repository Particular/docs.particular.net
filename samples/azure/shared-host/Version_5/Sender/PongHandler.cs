namespace Sender
{
    using NServiceBus;
    using Shared;

    public class PongHandler : IHandleMessages<Pong>
    {
        public void Handle(Pong message)
        {
            Logger.WriteLine("Sender", "Got Pong from Receiver");
        }
    }
}