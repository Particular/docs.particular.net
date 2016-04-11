namespace Raven_4.UpgradeGuides._3to4
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using Raven.Client;

    class SharedSessionEndpointConfig
    {
        Task DoStuff(EndpointConfiguration endpointConfiguration, IAsyncDocumentSession someAsyncSession)
        {
            #region 3to4-ravensharedsession
            Func<IAsyncDocumentSession> sessionFactory = () => someAsyncSession;

            endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .UseSharedAsyncSession(sessionFactory);
            #endregion

            return Task.FromResult(0);
        }
    }
}
