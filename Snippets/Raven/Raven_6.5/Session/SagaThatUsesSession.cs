namespace Raven_6.Session
{
    using System.Threading.Tasks;
    using NServiceBus;
    using Raven.Client.Documents.Session;

    #region ravendb-persistence-shared-session-for-saga

    public class SagaThatUsesSession :
        Saga<SagaThatUsesSession.SagaData>,
        IHandleMessages<MyMessage>
    {
        readonly IAsyncDocumentSession ravenSession;

        public SagaThatUsesSession(IAsyncDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var document = new MyDocument();
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