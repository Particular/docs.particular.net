using NServiceBus;
using NServiceBus.Saga;

namespace Snippets6.Sagas.FindByExpression
{
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

        public void Handle(Message1 message)
        {
            // code to handle Message1
        }

        public void Handle(Message2 message)
        {
            // code to handle Message2
        }
    }

}