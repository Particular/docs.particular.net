namespace Testing_7.HeaderManipulation
{
    using NServiceBus;

    class RequestMessage :
        IMessage
    {
        public string String { get; set; }
    }
}