using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.TransactionalSession;

class Worker : BackgroundService
{
    readonly IServiceProvider serviceProvider;
    readonly ILogger<Worker> logger;
    readonly IHostApplicationLifetime applicationLifetime;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger, IHostApplicationLifetime applicationLifetime)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
        this.applicationLifetime = applicationLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Opening a new transactional session");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var customerId = Guid.NewGuid().ToString();
                        var session = scope.ServiceProvider.GetRequiredService<ITransactionalSession>();

                        await session.Open(new CosmosOpenSessionOptions(new PartitionKey(customerId)), cancellationToken);

                        Console.WriteLine(
                            "Press 's' to create a new order, 'c' to commit or 'a' to abort the transaction");
                        var continueSending = true;
                        while (continueSending)
                        {
                            var key = Console.ReadKey();
                            cancellationToken.ThrowIfCancellationRequested();

                            switch (key.Key)
                            {
                                case ConsoleKey.S:
                                    var order = new OrderDocument
                                    {
                                        OrderId = Guid.NewGuid().ToString(),
                                        CustomerId = customerId,
                                        Status = "Received"
                                    };
                                    var storageSession = session.SynchronizedStorageSession.CosmosPersistenceSession();
                                    // store document atomically with the outgoing message
                                    storageSession.Batch.CreateItem(order);
                                    await session.Publish(
                                        new OrderReceived { OrderId = order.OrderId, CustomerId = customerId },
                                        cancellationToken);
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
            catch (Exception e)
            {
                logger.LogError(exception: e, message: "error processing user input:");
                Console.ReadKey();
                applicationLifetime.StopApplication();
            }
        });

        // Avoids blocking shutdown due to Console.ReadKey() calls
        return Task.CompletedTask;
    }
}