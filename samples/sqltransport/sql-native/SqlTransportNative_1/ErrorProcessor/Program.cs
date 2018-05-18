using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.Native.ErrorProcessor";
        Console.WriteLine("Press any key to exit");

        await CreateErrorQueue().ConfigureAwait(false);

        #region MessageProcessingLoop

        long startingRow;

        var rowVersionTracker = new RowVersionTracker();
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            await rowVersionTracker.CreateTable(connection).ConfigureAwait(false);
            startingRow = await rowVersionTracker.Get(connection).ConfigureAwait(false);
        }

        async Task Callback(SqlTransaction sqlTransaction, IncomingMessage message, CancellationToken cancellation)
        {
            using (var reader = new StreamReader(message.Body))
            {
                var bodyText = await reader.ReadToEndAsync().ConfigureAwait(false);
                Console.WriteLine($"Message received in error queue:\r\n{bodyText}");
            }
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message processing loop failed", exception);
        }

        Task PersistRowVersion(SqlTransaction transaction, long rowVersion, CancellationToken cancellation)
        {
            return rowVersionTracker.Save(transaction, rowVersion, cancellation);
        }

        Task<SqlTransaction> TransactionBuilder(CancellationToken cancellation)
        {
            return ConnectionHelpers.BeginTransaction(SqlHelper.ConnectionString, cancellation);
        }

        var processingLoop = new MessageProcessingLoop(
            table: "error",
            delay: TimeSpan.FromSeconds(1),
            transactionBuilder: TransactionBuilder,
            callback: Callback,
            errorCallback: ErrorCallback,
            startingRow: startingRow,
            persistRowVersion: PersistRowVersion);
        processingLoop.Start();

        Console.ReadKey();

        await processingLoop.Stop()
            .ConfigureAwait(false);

        #endregion
    }

    static async Task CreateErrorQueue()
    {
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            var queueManager = new QueueManager("error", connection);
            await queueManager.Create()
                .ConfigureAwait(false);
        }
    }
}