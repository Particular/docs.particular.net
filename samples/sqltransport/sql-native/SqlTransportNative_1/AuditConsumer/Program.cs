using System;
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

        await QueueCreator.Create(SqlHelper.ConnectionString, "audit").ConfigureAwait(false);

        #region MessageConsumingLoop

        Task Callback(IncomingBytesMessage incomingMessage, CancellationToken cancellation)
        {
            var bodyText = Encoding.UTF8.GetString(incomingMessage.Body);
            Console.WriteLine($"Message received in audit queue:\r\n{bodyText}");
            return Task.CompletedTask;
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message consuming loop failed", exception);
        }

        var consumingLoop = new MessageConsumingLoop(
            table: "audit",
            delay: TimeSpan.FromSeconds(1),
            connection: SqlHelper.ConnectionString,
            callback: Callback,
            errorCallback: ErrorCallback);
        consumingLoop.Start();

        Console.ReadKey();

        await consumingLoop.Stop()
            .ConfigureAwait(false);

        #endregion
    }
}