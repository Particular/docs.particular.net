using NServiceBus;
using NServiceBus.RavenDB.Persistence;

namespace Raven_3.Session
{

    #region ravendb-persistence-shared-session-for-handler

    public class HandlerThatUsesSession :
        IHandleMessages<MyMessage>
    {
        ISessionProvider sessionProvider;

        public HandlerThatUsesSession(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public void Handle(MyMessage message)
        {
            var doc = new MyDocument();

            sessionProvider.Session.Store(doc);
        }
    }

    #endregion

}