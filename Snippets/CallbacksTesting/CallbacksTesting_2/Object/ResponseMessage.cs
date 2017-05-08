namespace CallbacksTesting2.Object
{
    using NServiceBus;

    public class ResponseMessage :
        IMessage
    {
        public string Property { get; set; }
    }
}