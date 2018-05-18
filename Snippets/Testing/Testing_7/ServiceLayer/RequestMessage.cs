namespace Testing_7.ServiceLayer
{
    using NServiceBus;

    public class RequestMessage :
        IMessage
    {
        public string String { get; set; }
    }
}