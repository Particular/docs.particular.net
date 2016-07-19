namespace Testing_6.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}