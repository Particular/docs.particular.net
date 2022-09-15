using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
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

        public static async Task OpenDefault(IServiceProvider serviceProvider)
        {
            #region open-transactional-session-nhibernate

            using var childScope = serviceProvider.CreateScope();
            var session = childScope.ServiceProvider.GetService<ITransactionalSession>();
            await session.Open(new NHibernateOpenSessionOptions());

            await session.Send(new MyMessage());

            await session.Commit();

            #endregion
        }
    }
}