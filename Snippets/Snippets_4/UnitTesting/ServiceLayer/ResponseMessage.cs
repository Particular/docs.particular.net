namespace Snippets4.UnitTesting.ServiceLayer
{
    using NServiceBus;

    public class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }
}