namespace Snippets5.RavenDB.UpgradeGuides._3to4
{
    using global::Raven.Client;
    using NServiceBus;
    using NServiceBus.RavenDB.Persistence;
    using Snippets5.Handlers;

    #region 3to4-acccessingravenfromhandler
    public class HandlerWithRavenSession : IHandleMessages<MyMessage>
    {
        ISessionProvider sessionProvider;

        public HandlerWithRavenSession(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public void Handle(MyMessage message)
        {
            IDocumentSession ravenSession = sessionProvider.Session;
            SomeLibrary.SomeMethod(message, ravenSession);
        }
    }
    #endregion
}
