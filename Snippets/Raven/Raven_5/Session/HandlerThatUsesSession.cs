namespace Raven_5.Session
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region ravendb-persistence-shared-session-for-handler

    public class HandlerThatUsesSession :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var document = new MyDocument();
            var ravenSession = context.SynchronizedStorageSession.RavenSession();
            return ravenSession.StoreAsync(document);
        }
    }

    #endregion

}