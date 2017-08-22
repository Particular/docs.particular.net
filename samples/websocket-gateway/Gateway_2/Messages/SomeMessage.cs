using NServiceBus;

namespace Messages
{
    public class SomeMessage : IMessage
    {
        public string Contents { get; set; }
    }
}
