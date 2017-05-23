using NServiceBus;

namespace Messages
{
    public class MyMessage :
        IMessage
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}