using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence;
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

    public void ConfigureRemoteProcessor(EndpointConfiguration endpointConfiguration)
    {
        #region configure-remote-processor

        var transactionalSessionOptions = new TransactionalSessionOptions
        {
            ProcessorAddress = "MyRemoteProcessor"
        };

        endpointConfiguration.UsePersistence<MyPersistence>()
            .EnableTransactionalSession(transactionalSessionOptions);

        #endregion
    }
}

public static class TransactionalSessionConfigurationExtensions
{
    public static void EnableTransactionalSession(this PersistenceExtensions<MyPersistence> persistence, TransactionalSessionOptions transactionalSessionOptions = null)
    {
    }
}

//TODO: remove once we update to the new 3.3 package that contains this type
public class TransactionalSessionOptions
{
    public string ProcessorAddress { get; init; }
}

public class MyPersistence : PersistenceDefinition
{
}
