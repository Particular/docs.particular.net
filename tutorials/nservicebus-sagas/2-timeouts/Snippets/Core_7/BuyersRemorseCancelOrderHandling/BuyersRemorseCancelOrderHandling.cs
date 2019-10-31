namespace Core_7.BuyersRemorseCancelOrderHandling
{
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
            mapper.ConfigureMapping<PlaceOrder>(p => p.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<CancelOrder>(p => p.OrderId).ToSaga(s => s.OrderId);
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.OrderId} was cancelled.");

            //TODO: Update status in database?

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

    internal class SagaPropertyMapper<T>
    {
        internal SagaPropertyMapper<T> ConfigureMapping<T1>(Func<T1, object> p)
        {
            throw new NotImplementedException();
        }

        internal void ToSaga(Func<T, object> p)
        {
            throw new NotImplementedException();
        }
    }

    internal class OrderPlaced
    {
        public object CustomerId { get; set; }
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
        public object CustomerId { get; set; }
    }

    internal class CancelOrder
    {
        public object OrderId { get; set; }
    }
}
