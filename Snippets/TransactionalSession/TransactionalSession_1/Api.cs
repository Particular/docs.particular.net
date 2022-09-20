using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MyPersistence;
using NServiceBus.ObjectBuilder;
using NServiceBus.TransactionalSession;

class Api
{
    public void Enable(EndpointConfiguration config)
    {
        #region enabling-transactional-session

        //Each persistence has a specific Configure method
        var persistence = config.UsePersistence<MyPersistence>();
        persistence.EnableTransactionalSession();

        #endregion
    }

    public void Outbox(EndpointConfiguration config)
    {
        #region enabling-outbox

        config.EnableOutbox();

        #endregion
    }

    public async Task Open(IBuilder childBuilder, CancellationToken cancellationToken)
    {
        #region opening-transactional-session

        using var session = childBuilder.Build<ITransactionalSession>();
        await session.Open(new MyPersistenceOpenSessionOptions(),
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        #endregion

        #region configuring-timeout-transactional-session

        await session.Open(new MyPersistenceOpenSessionOptions
                {
                    MaximumCommitDuration = TimeSpan.FromSeconds(15)
                },
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        #endregion

        #region configuring-metadata-transactional-session

        await session.Open(new MyPersistenceOpenSessionOptions
                {
                    Metadata =
                    {
                        { "SomeKey", "SomeValue" }
                    }
                },
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

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

        await session.Commit(cancellationToken)
            .ConfigureAwait(false);

        #endregion
    }
}
