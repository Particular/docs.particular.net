using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;

namespace TransactionalSession_1
{
    public class MongoDBConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-mongodb

            config.UsePersistence<MongoPersistence>().EnableTransactionalSession();

            #endregion
        }

        public static async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-mongodb

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new MongoOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}