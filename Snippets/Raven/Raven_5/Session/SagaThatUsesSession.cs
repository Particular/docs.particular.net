namespace Raven_5.Session
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region ravendb-persistence-shared-session-for-saga

    public class SagaThatUsesSession :
        Saga<SagaThatUsesSession.SagaData>,
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var document = new MyDocument();
            var ravenSession = context.SynchronizedStorageSession.RavenSession();
            return ravenSession.StoreAsync(document);
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData :
            ContainSagaData
        {
        }
    }

}