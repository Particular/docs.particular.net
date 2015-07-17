namespace Snippets6.Callback
{
    using NServiceBus;

    class PlaceOrderResponse : IMessage
    {
        public object Response { get; set; }
    }
}