namespace Testing_7.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}