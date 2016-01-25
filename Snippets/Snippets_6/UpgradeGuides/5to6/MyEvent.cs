namespace Snippets6.Handlers
{
    using NServiceBus;

    public class MyEvent : IEvent
    {
        public string Data { get; set; }
    }
}