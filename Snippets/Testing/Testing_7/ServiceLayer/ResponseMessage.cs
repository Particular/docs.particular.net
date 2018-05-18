namespace Testing_7.ServiceLayer
{
    using NServiceBus;

    public class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}