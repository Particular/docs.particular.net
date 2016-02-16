namespace Snippets6.RavenDB.UpgradeGuides._3to4
{
    using System.Threading.Tasks;
    using global::Raven.Client;
    using NServiceBus;
    using Snippets6.Handlers;

    #region 3to4-acccessingravenfromhandler
    public class HandlerWithRavenSession : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            IAsyncDocumentSession ravenSession = context.SynchronizedStorageSession
                .RavenSession();
            await SomeLibrary.SomeAsyncMethod(message, ravenSession);
        }
    }
    #endregion
}
