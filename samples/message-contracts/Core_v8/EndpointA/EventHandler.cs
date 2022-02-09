namespace EndpointA
{
    using EndpointB.Contracts;
    using NServiceBus;

    public class EventHandler : IHandleMessages<EndpointBEvent> //handles V1
    {
        public Task Handle(EndpointBEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received event");
            return Task.CompletedTask;
        }
    }
}