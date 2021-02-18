namespace Raven_6.Session
{
    using System.Threading.Tasks;
    using NServiceBus;
    using Raven.Client.Documents.Session;

    #region ravendb-persistence-shared-session-for-handler

    public class HandlerThatUsesSession : IHandleMessages<MyMessage>
    {
        readonly IAsyncDocumentSession ravenSession;

        public HandlerThatUsesSession(IAsyncDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var document = new MyDocument();
            return ravenSession.StoreAsync(document);
        }
    }

    #endregion

}