using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class ProcessingLoop
{
    SqlConnection sqlConnection = null;
    string connectionString = null;

    async Task RowTracking()
    {

        long newRowVersion = 0;

        #region RowVersionTracker

        var versionTracker = new RowVersionTracker();

        // create table
        await versionTracker.CreateTable(sqlConnection)
            .ConfigureAwait(false);

        // save row version
        await versionTracker.Save(sqlConnection, newRowVersion)
            .ConfigureAwait(false);

        // get row version
        var startingRow = await versionTracker.Get(sqlConnection)
            .ConfigureAwait(false);

        #endregion
    }

    async Task ReadLoop()
    {
        #region ProcessingLoop

        var rowVersionTracker = new RowVersionTracker();

        var startingRow = await rowVersionTracker.Get(sqlConnection).ConfigureAwait(false);

        Task Callback(SqlTransaction transaction, IncomingBytesMessage message, CancellationToken Ccancellation)
        {
            var bodyText = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Message received in error message:\r\n{bodyText}");
            return Task.CompletedTask;
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message processing loop failed", exception);
        }

        Task<SqlTransaction> TransactionBuilder(CancellationToken cancellation)
        {
            return ConnectionHelpers.BeginTransaction(connectionString, cancellation);
        }

        Task PersistRowVersion(SqlTransaction transaction, long rowVersion, CancellationToken token)
        {
            return rowVersionTracker.Save(sqlConnection, rowVersion, token);
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

}