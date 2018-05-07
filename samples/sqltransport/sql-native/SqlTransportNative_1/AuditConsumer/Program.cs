using System;
using System.Data.SqlClient;
using System.Text;
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

        Task Callback(SqlTransaction transaction, IncomingBytesMessage message, CancellationToken cancellation)
        {
            var bodyText = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Message received in audit queue:\r\n{bodyText}");
            return Task.CompletedTask;
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