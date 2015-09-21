using NServiceBus;

namespace Snippets6.Sagas.SimpleSaga
{
    using System.Threading.Tasks;

    #region simple-saga

    public class OrderSaga : Saga<OrderSagaData>,
                            IAmStartedByMessages<StartOrder>,
                            IHandleMessages<CompleteOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<CompleteOrder>(s => s.OrderId)
                    .ToSaga(m => m.OrderId);
        }

        public async Task Handle(StartOrder message)
        {
            Data.OrderId = message.OrderId;
        }

        public async Task Handle(CompleteOrder message)
        {
            // code to handle order completion
            MarkAsComplete();
        }
    }

    #endregion

}
