using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.TransactionalSession;

class Worker : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var number = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Opening a new transactional session");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var session = scope.ServiceProvider.GetRequiredService<ITransactionalSession>();
                        await session.Open(cancellationToken: cancellationToken);

                        Console.WriteLine("Press 's' to send a message, 'c' to commit or 'a' to abort the transaction");
                        var continueSending = true;
                        while (continueSending)
                        {
                            var key = Console.ReadKey();
                            cancellationToken.ThrowIfCancellationRequested();

                            switch (key.Key)
                            {
                                case ConsoleKey.S:
                                    await session.SendLocal(new MyMessage { Number = ++number }, cancellationToken)
                                        .ConfigureAwait(false);
                                    break;
                                case ConsoleKey.C:
                                    await session.Commit(cancellationToken);
                                    continueSending = false;
                                    break;
                                case ConsoleKey.A:
                                    continueSending = false;
                                    break;
                            }
                        }

                        Console.WriteLine();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
        });

        // Avoids blocking shutdown due to Console.ReadKey() calls
        return Task.CompletedTask;
    }
}
