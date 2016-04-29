namespace Testing_5.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }
}