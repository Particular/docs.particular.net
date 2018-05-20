using NServiceBus;

namespace MyNamespace
{
    class MyMessage : IMessage
    {
        public string Property { get; set; }
    }
}