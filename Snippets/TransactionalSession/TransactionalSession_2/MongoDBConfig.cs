using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;
using System;
using System.Threading.Tasks;

namespace TransactionalSession_2
{
    public class MongoDBConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-mongodb

            config.UsePersistence<MongoPersistence>().EnableTransactionalSession();

            #endregion
        }

        public static async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-mongodb

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new MongoOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}