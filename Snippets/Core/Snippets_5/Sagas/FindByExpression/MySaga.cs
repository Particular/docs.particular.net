namespace Core5.Sagas.FindByExpression
{
    using NServiceBus;
    using NServiceBus.Saga;

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