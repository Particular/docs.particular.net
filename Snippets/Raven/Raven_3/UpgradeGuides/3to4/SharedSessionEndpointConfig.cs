using System;
using NServiceBus;
using NServiceBus.Persistence;
using System.Collections.Generic;
using Raven.Client;

class SharedSessionEndpointConfig
{
    SharedSessionEndpointConfig(BusConfiguration busConfiguration, IDocumentSession someSession)
    {
        #region 3to4-ravensharedsession
        Func<IDictionary<string, string>, IDocumentSession> sessionFactory = headers => someSession;

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedSession(sessionFactory);
        #endregion
    }
}