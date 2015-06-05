namespace Snippets3.Callback
{
    using NServiceBus;

    class PlaceOrderResponse : IMessage
    {
        public object Response { get; set; }
    }
}