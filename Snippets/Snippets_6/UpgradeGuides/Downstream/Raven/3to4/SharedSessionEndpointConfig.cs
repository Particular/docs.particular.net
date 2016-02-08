namespace Snippets6.UpgradeGuides.Downstream.Raven._3to4
{
    using System;
    using System.Threading.Tasks;
    using global::Raven.Client;
    using NServiceBus;

    public class SharedSessionEndpointConfig
    {
        public async Task DoStuff(EndpointConfiguration configuration, IAsyncDocumentSession someAsyncSession)
        {
            #region 3to4-ravensharedsession
            Func<IAsyncDocumentSession> sessionFactory = () => someAsyncSession;

            configuration.UsePersistence<RavenDBPersistence>().UseSharedAsyncSession(sessionFactory);
            #endregion
        }
    }
}
