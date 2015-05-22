namespace Sender
{
    using System.Diagnostics;
    using NServiceBus;

    public class PongHandler : IHandleMessages<Pong>
    {
        public void Handle(Pong message)
        {
            Trace.WriteLine("Received Pong with message: " + message.Message);
        }
    }
}