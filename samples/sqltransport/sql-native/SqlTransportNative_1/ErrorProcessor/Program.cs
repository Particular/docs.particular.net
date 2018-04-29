using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.Native.ErrorProcessor";
        Console.WriteLine("Press any key to exit");

        await QueueCreator.Create(SqlHelper.ConnectionString, "error")
            .ConfigureAwait(false);

        #region MessageProcessingLoop

        var rowVersionTracker = new RowVersionTracker();
        await rowVersionTracker.CreateTable(SqlHelper.ConnectionString).ConfigureAwait(false);
        var startingRow = await rowVersionTracker.Get(SqlHelper.ConnectionString).ConfigureAwait(false);

        Task Callback(IncomingBytesMessage incomingMessage, CancellationToken cancellation)
        {
            var bodyText = Encoding.UTF8.GetString(incomingMessage.Body);
            Console.WriteLine($"Message received in error queue:\r\n{bodyText}");
            return Task.CompletedTask;
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message processing loop failed", exception);
        }

        Task PersistRowVersion(long rowVersion, CancellationToken token)
        {
            return rowVersionTracker.Save(SqlHelper.ConnectionString, rowVersion, token);
        }

        var processingLoop = new MessageProcessingLoop(
            table: "error",
            delay: TimeSpan.FromSeconds(1),
            connection: SqlHelper.ConnectionString,
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