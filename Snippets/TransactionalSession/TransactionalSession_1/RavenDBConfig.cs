using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransactionalSession_1
{
    public class RavenDBConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-ravendb

            var persistence = config.UsePersistence<RavenDBPersistence>();
            persistence.EnableTransactionalSession();

            #endregion
        }

        private async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-ravendb

            using var childBuilder = builder.CreateChildBuilder();
            var session = childBuilder.Build<ITransactionalSession>();
            await session.Open(new RavenDbOpenSessionOptions()).ConfigureAwait(false);

            // use the session

            await session.Commit().ConfigureAwait(false);

            #endregion
        }

        public async Task OpenMultiTenant(IBuilder builder)
        {
            #region open-transactional-session-ravendb-multitenant

            using var childBuilder = builder.CreateChildBuilder();
            var session = childBuilder.Build<ITransactionalSession>();
            await session.Open(new RavenDbOpenSessionOptions(new Dictionary<string, string>
            {
                // information is added to the message headers for the 
                // `SetMessageToDatabaseMappingConvention`-method
                {"tenantDatabaseName", "tenantA-databaseName"}
            })).ConfigureAwait(false);

            // use the session

            await session.Commit().ConfigureAwait(false);

            #endregion
        }

        public async Task UseSession(ITransactionalSession session)
        {
            #region use-transactional-session-raven
            await session.Open(new RavenDbOpenSessionOptions()).ConfigureAwait(false);

            // add messages to the transaction:
            await session.Send(new MyMessage()).ConfigureAwait(false);

            // access the database:
            var ravenSession = session.SynchronizedStorageSession.RavenSession();

            await session.Commit().ConfigureAwait(false);
            #endregion
        }
    }
}