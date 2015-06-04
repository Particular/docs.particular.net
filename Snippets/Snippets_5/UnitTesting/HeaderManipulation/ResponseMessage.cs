namespace Snippets5.UnitTesting.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }
}