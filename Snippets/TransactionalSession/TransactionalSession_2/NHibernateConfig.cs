using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;

namespace TransactionalSession_2
{
    public class NHibernateConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-nhibernate

            config.UsePersistence<NHibernatePersistence>().EnableTransactionalSession();

            #endregion
        }

        public async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-nhibernate

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new NHibernateOpenSessionOptions());

            // use the session

            await session.Commit();

            #endregion
        }

        public async Task UseSession(ITransactionalSession session)
        {
            #region use-transactional-session-nhibernate
            await session.Open(new NHibernateOpenSessionOptions());

            // add messages to the transaction:
            await session.Send(new MyMessage());

            // access the database:
            var nhibernateSession = session.SynchronizedStorageSession.Session();

            await session.Commit();
            #endregion
        }
    }
}