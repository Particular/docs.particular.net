using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransactionalSession_2
{
    public class RavenDBConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-ravendb

            config.UsePersistence<RavenDBPersistence>().EnableTransactionalSession();

            #endregion
        }

        private static async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-ravendb

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new RavenDbOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }

        public async Task OpenMultiTenant(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-ravendb-multitenant

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
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