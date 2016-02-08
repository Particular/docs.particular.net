namespace Snippets6.UpgradeGuides.Downstream.Raven._3to4
{
    using System;
    using global::Raven.Client;
    using NServiceBus;
    using NServiceBus.Persistence;

    public class SharedSessionEndpointConfig
    {
        public void DoStuff(BusConfiguration configuration, IDocumentSession someSession)
        {
            #region 3to4-ravensharedsession
            Func<IDocumentSession> sessionFactory = () => someSession;

            configuration.UsePersistence<RavenDBPersistence>().UseSharedSession(sessionFactory);
            #endregion
        }
    }
}
