using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransactionalSession_1
{
    public class RavenDBConfig
    {
        public async Task Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-ravendb

            config.UsePersistence<RavenDBPersistence>().EnableTransactionalSession();

            #endregion

            IBuilder builder = null;

            await OpenDefault(builder);
        }

        private static async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-ravendb

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new RavenDbOpenSessionOptions());

            await session.Send(new MyMessage());

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

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}