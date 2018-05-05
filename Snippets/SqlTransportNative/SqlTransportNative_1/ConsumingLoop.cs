using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class ConsumingLoop
{
    string connectionString = null;

    async Task ConsumeLoop()
    {
        #region ConsumeLoop

        Task Callback(SqlTransaction transaction, IncomingBytesMessage message, CancellationToken cancellation)
        {
            var bodyText = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Reply received:\r\n{bodyText}");
            return Task.CompletedTask;
        }

        Task<SqlTransaction> TransactionBuilder(CancellationToken cancellation)
        {
            return ConnectionHelpers.BeginTransaction(connectionString, cancellation);
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message consuming loop failed", exception);
        }

        // start consuming
        var consumingLoop = new MessageConsumingLoop(
            table: "endpointTable",
            delay: TimeSpan.FromSeconds(1),
            transactionBuilder: TransactionBuilder,
            callback: Callback,
            errorCallback: ErrorCallback);
        consumingLoop.Start();

        // stop consuming
        await consumingLoop.Stop().ConfigureAwait(false);

        #endregion
    }
}