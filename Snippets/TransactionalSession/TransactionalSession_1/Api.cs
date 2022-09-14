using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.MyPersistence;
using NServiceBus.TransactionalSession;

class Api
{
    public void Enable(EndpointConfiguration config)
    {
        #region enabling-transactional-session

        //Each persistence has a specific Configure method
        config.UsePersistence<MyPersistence>().EnableTransactionalSession();

        #endregion
    }

    public async Task Open(IServiceScope scope, CancellationToken cancellationToken)
    {
        #region opening-transactional-session

        var session = scope.ServiceProvider.GetRequiredService<ITransactionalSession>();
        await session.Open(new MyPersistenceOpenSessionOptions(),
            cancellationToken: cancellationToken);

        #endregion
    }

    public async Task Send(ITransactionalSession session, CancellationToken cancellationToken)
    {
        #region sending-transactional-session

        await session.SendLocal(new MyMessage(), cancellationToken)
            .ConfigureAwait(false);

        #endregion

    }

    public async Task Commit(ITransactionalSession session, CancellationToken cancellationToken)
    {
        #region committing-transactional-session

        await session.Commit(cancellationToken);

        #endregion
    }


}
