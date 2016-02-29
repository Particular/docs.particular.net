namespace Snippets5.RavenDB.UpgradeGuides._3to4
{
    using System;
    using global::Raven.Client;
    using NServiceBus;
    using NServiceBus.Persistence;

    public class SharedSessionEndpointConfig
    {
        public void DoStuff(BusConfiguration busConfiguration, IDocumentSession someSession)
        {
            #region 3to4-ravensharedsession
            Func<IDocumentSession> sessionFactory = () => someSession;

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .UseSharedSession(sessionFactory);
            #endregion
        }
    }
}
