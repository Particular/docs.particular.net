namespace Core_7.BuyersRemorseCancelOrderHandling
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;

    #region BuyersRemorseCancelOrderHandling

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>,
        IHandleTimeouts<BuyersRemorseIsOver>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.MapSaga(saga => saga.OrderId)
                .ToMessage<PlaceOrder>(message => message.OrderId)
                .ToMessage<CancelOrder>(message => message.OrderId);
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.OrderId} was cancelled.");

            //TODO: Possibly publish an OrderCancelled event?

            MarkAsComplete();

            return Task.CompletedTask;
        }
    }

    #endregion

    internal interface IHandleTimeouts<T>
    {
    }

    internal interface IHandleMessages<T>
    {
    }

    internal interface IAmStartedByMessages<T>
    {
    }

    public interface IMessageHandlerContext
    {
    }

    internal class Saga<T>
    {
        protected virtual void ConfigureHowToFindSaga(SagaPropertyMapper<T> mapper) { }

        protected void MarkAsComplete()
        {
        }
    }

    internal class SagaPropertyMapper<TSagaData>
    {
        internal SagaPropertyMapper<TSagaData> MapSaga(Func<TSagaData, object> p)
        {
            return this;
        }

        internal SagaPropertyMapper<TSagaData> ToMessage<T>(Func<T, object> p)
        {
            return this;
        }
    }

    internal class OrderPlaced
    {
        public object OrderId { get; set; }
    }

    internal class BuyersRemorseIsOver
    {
    }

    internal class PlaceOrder
    {
        public object OrderId { get; internal set; }
    }

    internal class BuyersRemorseState
    {
        public object OrderId { get; set; }
    }

    internal class CancelOrder
    {
        public object OrderId { get; set; }
    }
}
