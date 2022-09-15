using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;

namespace TransactionalSession_2
{
    public class SqlPersistenceConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-sqlp

            config.UsePersistence<SqlPersistence>().EnableTransactionalSession();

            #endregion
        }

        public static async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-sqlp

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new SqlPersistenceOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }

        public async Task OpenMultiTenant(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-sqlp-multitenant

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new SqlPersistenceOpenSessionOptions((
                "MyTenantIdHeader", //Name of the header configured in this endpoint to carry the tenant ID
                "TenantA")));       //The value of the tenant ID header

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}