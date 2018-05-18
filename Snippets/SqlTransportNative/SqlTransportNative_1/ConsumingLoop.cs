using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class ConsumingLoop
{
    string connectionString = null;

    async Task ConsumeLoop()
    {
        #region ConsumeLoop

        async Task Callback(SqlTransaction transaction, IncomingMessage message, CancellationToken cancellation)
        {
            using (var reader = new StreamReader(message.Body))
            {
                var bodyText = await reader.ReadToEndAsync()
                    .ConfigureAwait(false);
                Console.WriteLine($"Reply received:\r\n{bodyText}");
            }
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
        await consumingLoop.Stop()
            .ConfigureAwait(false);

        #endregion
    }
}