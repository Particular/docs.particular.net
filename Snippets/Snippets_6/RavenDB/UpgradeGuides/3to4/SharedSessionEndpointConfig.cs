namespace Snippets6.RavenDB.UpgradeGuides._3to4
{
    using System;
    using System.Threading.Tasks;
    using global::Raven.Client;
    using NServiceBus;

    class SharedSessionEndpointConfig
    {
        async Task DoStuff(EndpointConfiguration endpointConfiguration, IAsyncDocumentSession someAsyncSession)
        {
            #region 3to4-ravensharedsession
            Func<IAsyncDocumentSession> sessionFactory = () => someAsyncSession;

            endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .UseSharedAsyncSession(sessionFactory);
            #endregion
        }
    }
}
