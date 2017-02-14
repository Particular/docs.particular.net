using NServiceBus;
using NServiceBus.Saga;
using NServiceBus.RavenDB.Persistence;

namespace Raven_3.Session
{

    #region ravendb-persistence-shared-session-for-saga

    public class SagaThatUsesSession :
        Saga<SagaThatUsesSession.SagaData>,
        IHandleMessages<MyMessage>
    {
        ISessionProvider sessionProvider;

        public SagaThatUsesSession(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public void Handle(MyMessage message)
        {
            var document = new MyDocument();
            sessionProvider.Session.Store(document);
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