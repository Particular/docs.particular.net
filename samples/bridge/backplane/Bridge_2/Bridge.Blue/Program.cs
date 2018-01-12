using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;
using NServiceBus.Extensibility;
using NServiceBus.Transport;

class Deduplicator
{
    public Task Deduplicate(string queue, MessageContext message, Dispatch dispatch, Func<Dispatch, Task> forward)
    {
        async Task DeduplicateDispatch(TransportOperations messages, TransportTransaction transaction, ContextBag context)
        {
            using (var conn = new SqlConnection(ConnectionStrings.Blue))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var tx = conn.BeginTransaction())
                {
                    var duplicateOps = new List<UnicastTransportOperation>();

                    //Detect duplicates
                    foreach (var operation in messages.UnicastTransportOperations)
                    {
                        if (await WasForwarded(conn, tx, operation.Message.MessageId))
                        {
                            duplicateOps.Add(operation);
                        }
                    }

                    //Remove duplicates
                    foreach (var duplicateOp in duplicateOps)
                    {
                        messages.UnicastTransportOperations.Remove(duplicateOp);
                    }

                    //Set the connection + transaction and dispatch
                    var forwardTransaction = new TransportTransaction();
                    forwardTransaction.Set(conn);
                    forwardTransaction.Set(tx);
                    await dispatch(messages, forwardTransaction, context).ConfigureAwait(false);

                    //Mark as processed
                    foreach (var operation in messages.UnicastTransportOperations)
                    {
                        await MarkAsForwarded(conn, tx, operation.Message.MessageId);
                    }
                    tx.Commit();
                }
            }
        }

        if (message.TransportTransaction.TryGet<SqlConnection>(out var _))
        {
            return forward(dispatch);
        }
        return forward(DeduplicateDispatch);
    }

    static async Task<bool> WasForwarded(SqlConnection conn, SqlTransaction tx, string messageId)
    {
        using (var command = conn.CreateCommand())
        {
            command.Transaction = tx;
            command.CommandText = "SELECT COUNT(*) FROM ReceivedMessages WHERE [Id] = @Id";
            command.Parameters.AddWithValue("@Id", messageId);
            var count = await command.ExecuteScalarAsync().ConfigureAwait(false);

            return (int)count == 1;
        }
    }

    static async Task MarkAsForwarded(SqlConnection conn, SqlTransaction tx, string messageId)
    {
        using (var command = conn.CreateCommand())
        {
            command.Transaction = tx;
            command.CommandText = "INSERT INTO ReceivedMessages ([Id]) VALUES (@Id)";
            command.Parameters.AddWithValue("@Id", messageId);
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}

class Program
{
    static async Task Main()
    {
        Console.Title = "Bridge.Blue";

        #region BridgeConfig

        //Same name, different transports
        var bridgeConfig = Bridge
            .Between<SqlServerTransport>("Samples.Bridge.Backplane.Bridge.Blue", t =>
            {
                t.ConnectionString(ConnectionStrings.Blue);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Samples.Bridge.Backplane.Bridge.Blue.Rabbit", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence<InMemoryPersistence>((config, persistence) => { });

        bridgeConfig.Forwarding.ForwardTo("MyMessage", "Samples.Bridge.Backplane.Bridge.Red.Rabbit");
        bridgeConfig.Forwarding.RegisterPublisher("MyEvent", "Samples.Bridge.Backplane.Bridge.Red.Rabbit");

        //bridgeConfig.LimitMessageProcessingConcurrencyTo(1);

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Blue);

        bridgeConfig.InterceptForwarding((queue, message, dispatch, forward) => new Deduplicator().Deduplicate(queue, message, dispatch, forward));

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }

    
}