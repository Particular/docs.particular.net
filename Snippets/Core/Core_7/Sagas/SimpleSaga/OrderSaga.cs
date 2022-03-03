#pragma warning disable NSB0004 // Saga mapping expressions can be simplified - Not available until NServiceBus 7.4

namespace Core7.Sagas.SimpleSaga
{
    using NServiceBus;
    using System.Threading.Tasks;

    #region simple-saga

    public class OrderSaga :
        Saga<OrderSagaData>,
        IAmStartedByMessages<StartOrder>,
        IHandleMessages<CompleteOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }

        public Task Handle(StartOrder message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CompleteOrder message, IMessageHandlerContext context)
        {
            // code to handle order completion
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }

    #endregion

}
#pragma warning restore NSB0004 // Saga mapping expressions can be simplified