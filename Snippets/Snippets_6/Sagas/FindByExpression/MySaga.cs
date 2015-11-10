using NServiceBus;

namespace Snippets6.Sagas.FindByExpression
{
    using System.Threading.Tasks;

    public class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<Message1>,
        IHandleMessages<Message2>
    {
        #region saga-find-by-expression

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.ConfigureMapping<Message2>(message => message.Part1 + "_" + message.Part2)
                .ToSaga(sagaData => sagaData.SomeID);
        }

        #endregion

        public async Task Handle(Message1 message, IMessageHandlerContext context)
        {
            // code to handle Message1
        }

        public async Task Handle(Message2 message, IMessageHandlerContext context)
        {
            // code to handle Message2
        }
    }

}