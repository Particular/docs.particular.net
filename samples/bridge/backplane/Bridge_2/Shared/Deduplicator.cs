using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Bridge;
using NServiceBus.Extensibility;
using NServiceBus.Transport;

public class Deduplicator
{
    string connectionString;

    public Deduplicator(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Task DeduplicateSQLMessages(string queue, MessageContext message, Dispatch dispatch, Func<Dispatch, Task> forward)
    {
        #region Deduplicate

        async Task DeduplicateDispatch(TransportOperations messages,
            TransportTransaction transaction, ContextBag context)
        {
            using (var conn = await OpenConnection().ConfigureAwait(false))
            using (var tx = conn.BeginTransaction())
            {
                var duplicateOps = new List<UnicastTransportOperation>();

                //Detect duplicates
                foreach (var operation in messages.UnicastTransportOperations)
                {
                    var messageId = operation.Message.MessageId;
                    if (await WasForwarded(conn, tx, messageId).ConfigureAwait(false))
                    {
                        duplicateOps.Add(operation);
                    }
                }

                //Remove duplicates
                foreach (var duplicateOp in duplicateOps)
                {
                    messages.UnicastTransportOperations.Remove(duplicateOp);
                }

                //Set the connection and transaction for the outgoing op
                var forwardTransaction = new TransportTransaction();
                forwardTransaction.Set(conn);
                forwardTransaction.Set(tx);

                //Dispatch
                await dispatch(messages, forwardTransaction, context)
                    .ConfigureAwait(false);

                //Mark as processed (atomically with dispatch)
                foreach (var operation in messages.UnicastTransportOperations)
                {
                    var messageId = operation.Message.MessageId;
                    await MarkAsForwarded(conn, tx, messageId).ConfigureAwait(false);
                }
                tx.Commit();
            }
        }

        #endregion

        return forward(message.TransportTransaction.TryGet<SqlConnection>(out var _) 
            ? dispatch 
            : DeduplicateDispatch);
    }

    async Task<SqlConnection> OpenConnection()
    {
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync().ConfigureAwait(false);
        return connection;
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

    static StringBuilder LogIncoming(MessageContext message)
    {
        var builder = new StringBuilder();
        builder.Append($"Forwarding message {message.MessageId}{Environment.NewLine}");
        return builder;
    }

    static void LogDispatch(StringBuilder builder, TransportOperations messages)
    {
        var allOperations = messages.UnicastTransportOperations
            .Cast<IOutgoingTransportOperation>()
            .Concat(messages.MulticastTransportOperations);

        foreach (var op in allOperations)
        {
            builder.Append($"   Dispatching message {op.Message.MessageId}{Environment.NewLine}");
        }
    }
}