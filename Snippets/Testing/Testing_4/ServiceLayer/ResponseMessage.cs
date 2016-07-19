namespace Testing_4.ServiceLayer
{
    using NServiceBus;

    public class ResponseMessage :
        IMessage
    {
        public string String { get; set; }
    }
}