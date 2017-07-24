#pragma warning disable 1998
namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using Core6.Sagas.SimpleSaga;
    using NServiceBus;

    #region 5to6-SagaDefinition

    public class OrderSaga :
            Saga<OrderSagaData>,
            IAmStartedByMessages<StartOrder>,
            IHandleMessages<CompleteOrder>

        #endregion

    {
        #region 5to6-ConfigureHowToFindSaga

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }

        #endregion

        #region 5to6-NoSagaDataCorrelationNeeded

        public async Task Handle(StartOrder message, IMessageHandlerContext context)
        {
            // The processing logic for the StartOrder message
        }

        #endregion

        public Task Handle(CompleteOrder message, IMessageHandlerContext context)
        {
            // code to handle order completion
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }
}