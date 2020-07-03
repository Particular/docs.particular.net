using NServiceBus;

namespace Messages
{
    public class SomeMessage : ICommand
    {
        public int Number { get; set; }
    }
}
