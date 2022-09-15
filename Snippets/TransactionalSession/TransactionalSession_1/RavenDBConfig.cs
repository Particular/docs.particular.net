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

            config.UsePersistence<RavenDBPersistence>().EnableTransactionalSession();

            #endregion
        }

        private async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-ravendb

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new RavenDbOpenSessionOptions());

            // use the session

            await session.Commit();

            #endregion
        }

        public async Task OpenMultiTenant(IBuilder builder)
        {
            #region open-transactional-session-ravendb-multitenant

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new RavenDbOpenSessionOptions(new Dictionary<string, string>
            {
                // information is added to the message headers for the `SetMessageToDatabaseMappingConvention`-method
                {"tenantDatabaseName", "tenantA-databaseName"}
            }));

            // use the session

            await session.Commit();

            #endregion
        }

        public async Task UseSession(ITransactionalSession session)
        {
            #region use-transactional-session-raven
            await session.Open(new RavenDbOpenSessionOptions());

            // add messages to the transaction:
            await session.Send(new MyMessage());

            // access the database:
            var ravenSession = session.SynchronizedStorageSession.RavenSession();

            await session.Commit();
            #endregion
        }
    }
}