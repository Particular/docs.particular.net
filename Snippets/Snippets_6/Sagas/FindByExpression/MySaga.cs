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
            mapper.ConfigureMapping<Message2>(m => m.Part1 + "_" + m.Part2)
                .ToSaga(m => m.SomeID);
        }

        #endregion

        public Task Handle(Message1 message)
        {
            // code to handle Message1
            return Task.FromResult(0);
        }

        public Task Handle(Message2 message)
        {
            // code to handle Message2
            return Task.FromResult(0);
        }
    }

}