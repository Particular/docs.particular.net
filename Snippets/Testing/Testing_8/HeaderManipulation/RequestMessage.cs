namespace Testing_8.HeaderManipulation
{
    using NServiceBus;

    class RequestMessage :
        IMessage
    {
        public string String { get; set; }
    }
}