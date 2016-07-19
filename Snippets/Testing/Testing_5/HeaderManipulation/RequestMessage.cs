namespace Testing_5.HeaderManipulation
{
    using NServiceBus;

    class RequestMessage :
        IMessage
    {
        public string String { get; set; }
    }
}