namespace Raven_3.UpgradeGuides._3to4
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;
    using Raven.Client;

    class SharedSessionEndpointConfig
    {
        SharedSessionEndpointConfig(BusConfiguration busConfiguration, IDocumentSession someSession)
        {
            #region 3to4-ravensharedsession
            Func<IDocumentSession> sessionFactory = () => someSession;

            var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.UseSharedSession(sessionFactory);
            #endregion
        }
    }
}
