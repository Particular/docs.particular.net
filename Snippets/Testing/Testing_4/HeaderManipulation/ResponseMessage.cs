namespace Testing_4.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}