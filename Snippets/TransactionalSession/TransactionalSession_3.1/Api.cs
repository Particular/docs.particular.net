using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.TransactionalSession;

class Api
{
    public async Task Open(IServiceScope scope, CancellationToken cancellationToken)
    {
        var session = scope.ServiceProvider.GetRequiredService<ITransactionalSession>();

        #region configuring-commit-delay-transactional-session

        await session.Open(new MyPersistenceOpenSessionOptions
            {
                CommitDelayIncrement = TimeSpan.FromSeconds(1)
            },
            cancellationToken: cancellationToken);

        #endregion
    }
}
