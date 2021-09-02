namespace Testing_8.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}