namespace Snippets6.UnitTesting.HeaderManipulation
{
    using NServiceBus;

    class RequestMessage : IMessage
    {
        public string String { get; set; }
    }
}