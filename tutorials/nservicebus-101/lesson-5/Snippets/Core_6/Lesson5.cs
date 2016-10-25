using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Core_6
{
    public class PlaceOrder : ICommand
    {
        public string OrderId { get; set; }
    }

    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
    }

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private static ILog logger = LogManager.GetLogger<PlaceOrderHandler>();

#pragma warning disable CS0162 // Unreachable code detected

        #region Throw
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            // This is normally where some database logic would occur
            throw new Exception("BOOM!");

            var orderPlaced = new OrderPlaced { OrderId = message.OrderId };
            return context.Publish(orderPlaced);
        }
        #endregion

#pragma warning restore CS0162 // Unreachable code detected
    }

    public class ConfigRetries
    {
        public void Immediate(EndpointConfiguration endpointConfiguration)
        {
            #region ImmediateRetries
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
            #endregion
        }

        public void Delayed(EndpointConfiguration endpointConfiguration)
        {
            #region DelayedRetries
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(0));

            recoverability.Delayed(delayed => delayed.NumberOfRetries(0));
            #endregion
        }

        public void TimeIncrease(EndpointConfiguration endpointConfiguration)
        {
            #region TimeIncrease
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(0));

            recoverability.Delayed(delayed =>
            {
                delayed.NumberOfRetries(3);
                delayed.TimeIncrease(TimeSpan.FromSeconds(3));
            });
            #endregion
        }
    }

}