using System;
using NServiceBus;
using Raven.Client;

class SharedSessionEndpointConfig
{
    void DoStuff(EndpointConfiguration endpointConfiguration, IAsyncDocumentSession someAsyncSession)
    {
        #region 3to4-ravensharedsession
        Func<IAsyncDocumentSession> sessionFactory = () => someAsyncSession;

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedAsyncSession(sessionFactory);
        #endregion
    }
}