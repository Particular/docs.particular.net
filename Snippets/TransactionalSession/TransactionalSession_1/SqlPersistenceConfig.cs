using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;

namespace TransactionalSession_1
{
    public class SqlPersistenceConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-sqlp

            config.UsePersistence<SqlPersistence>().EnableTransactionalSession();

            #endregion
        }

        public static async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-sqlp

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new SqlPersistenceOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }

        public async Task OpenMultiTenant(IBuilder builder)
        {
            #region open-transactional-session-sqlp-multitenant

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new SqlPersistenceOpenSessionOptions((
                "MyTenantIdHeader", //Name of the header configured in this endpoint to carry the tenant ID
                "TenantA")));       //The value of the tenant ID header

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}