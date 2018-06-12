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
        Console.Title = "Samples.SqlServer.Native.AuditConsumer";
        Console.WriteLine("Press any key to exit");

        await CreateAuditQueue().ConfigureAwait(false);

        #region MessageConsumingLoop

        async Task Callback(SqlTransaction transaction, IncomingMessage message, CancellationToken cancellation)
        {
            using (var reader = new StreamReader(message.Body))
            {
                var bodyText = await reader.ReadToEndAsync().ConfigureAwait(false);
                Console.WriteLine($"Message received in audit queue:\r\n{bodyText}");
            }
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message consuming loop failed", exception);
        }

        Task<SqlTransaction> TransactionBuilder(CancellationToken cancellation)
        {
            return ConnectionHelpers.BeginTransaction(SqlHelper.ConnectionString, cancellation);
        }

        var consumingLoop = new MessageConsumingLoop(
            table: "audit",
            delay: TimeSpan.FromSeconds(1),
            transactionBuilder: TransactionBuilder,
            callback: Callback,
            errorCallback: ErrorCallback);
        consumingLoop.Start();

        Console.ReadKey();

        await consumingLoop.Stop()
            .ConfigureAwait(false);

        #endregion
    }

    static async Task CreateAuditQueue()
    {
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            var queueManager = new QueueManager("audit", connection);
            await queueManager.Create()
                .ConfigureAwait(false);
        }
    }
}