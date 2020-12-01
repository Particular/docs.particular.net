namespace Core_7.Lesson1.Intro
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

#pragma warning disable 1998

    #region EmptyShippingPolicy
    public class ShippingPolicy :
        IHandleMessages<OrderPlaced>,
        IHandleMessages<OrderBilled>
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId}");
            return Task.CompletedTask;
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderBilled, OrderId = {message.OrderId}");
            return Task.CompletedTask;
        }
    }
    #endregion

#pragma warning restore 1998
}
