using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;

public class NHibernateConfig
{
    public void Configure(EndpointConfiguration config)
    {
        #region enabling-transactional-session-nhibernate

        var persistence = config.UsePersistence<NHibernatePersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public async Task OpenDefault(IServiceProvider serviceProvider)
    {
        #region open-transactional-session-nhibernate

        using var childScope = serviceProvider.CreateScope();
        var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
        await session.Open()
            .ConfigureAwait(false);

        // use the session

        await session.Commit()
            .ConfigureAwait(false);

        #endregion
    }

    public async Task UseSession(ITransactionalSession session)
    {
        #region use-transactional-session-nhibernate
        await session.Open()
            .ConfigureAwait(false);

        // add messages to the transaction:
        await session.Send(new MyMessage())
            .ConfigureAwait(false);

        // access the database:
        var nhibernateSession = session.SynchronizedStorageSession.Session();

        await session.Commit()
            .ConfigureAwait(false);
        #endregion
    }
}
