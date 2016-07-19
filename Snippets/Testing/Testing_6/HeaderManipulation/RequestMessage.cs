namespace Testing_6.HeaderManipulation
{
    using NServiceBus;

    class RequestMessage :
        IMessage
    {
        public string String { get; set; }
    }
}