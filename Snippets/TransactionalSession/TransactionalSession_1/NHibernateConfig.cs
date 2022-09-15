using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;
using System.Threading.Tasks;

namespace TransactionalSession_1
{
    public class NHibernateConfig
    {
        public void Configure(EndpointConfiguration config)
        {
            #region enabling-transactional-session-nhibernate

            config.UsePersistence<NHibernatePersistence>().EnableTransactionalSession();

            #endregion
        }

        public async Task OpenDefault(IBuilder builder)
        {
            #region open-transactional-session-nhibernate

            using var childScope = builder.CreateChildBuilder();
            var session = childScope.Build<ITransactionalSession>();
            await session.Open(new NHibernateOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}