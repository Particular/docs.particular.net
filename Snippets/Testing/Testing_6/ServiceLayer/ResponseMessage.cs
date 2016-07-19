namespace Testing_6.ServiceLayer
{
    using NServiceBus;

    public class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}