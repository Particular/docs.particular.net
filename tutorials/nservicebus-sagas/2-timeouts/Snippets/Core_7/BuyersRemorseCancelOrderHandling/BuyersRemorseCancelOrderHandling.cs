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
            mapper.ConfigureMapping<PlaceOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
            mapper.ConfigureMapping<CancelOrder>(message => message.OrderId).ToSaga(saga => saga.OrderId);
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
