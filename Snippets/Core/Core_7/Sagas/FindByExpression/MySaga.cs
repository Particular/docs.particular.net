using NServiceBus;

namespace Core6.Sagas.FindByExpression
{
    using System.Threading.Tasks;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<MyMessage>
    {
        #region saga-find-by-expression

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<MyMessage>(message => $"{message.Part1}_{message.Part2}")
                .ToSaga(sagaData => sagaData.SomeId);
        }

        #endregion

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
        }
    }

}