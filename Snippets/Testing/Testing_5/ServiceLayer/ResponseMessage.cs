namespace Snippets5.UnitTesting.ServiceLayer
{
    using NServiceBus;

    public class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }
}