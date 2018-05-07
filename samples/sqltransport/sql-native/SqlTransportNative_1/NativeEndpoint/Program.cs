using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.Native.NativeEndpoint";
        Console.WriteLine("Press s to send a message that will succeed");
        Console.WriteLine("Press f to send a message that will fail");
        Console.WriteLine("Press any key to exit");

        await CreateQueue().ConfigureAwait(false);

        #region receive

        Task Callback(SqlTransaction sqlTransaction, IncomingBytesMessage message, CancellationToken cancellation)
        {
            var bodyText = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Reply received:\r\n{bodyText}");
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

        var messageConsumingLoop = new MessageConsumingLoop(
            table: "NativeEndpoint",
            delay: TimeSpan.FromSeconds(1),
            transactionBuilder: TransactionBuilder,
            callback: Callback,
            errorCallback: ErrorCallback);
        messageConsumingLoop.Start();

        #endregion

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.S)
            {
                await SendMessageThatWillSucceed()
                    .ConfigureAwait(false);
                continue;
            }

            if (key.Key == ConsoleKey.F)
            {
                await SendMessageThatWillFail()
                    .ConfigureAwait(false);
                continue;
            }

            break;
        }

        await messageConsumingLoop.Stop()
            .ConfigureAwait(false);
    }

    static Task SendMessageThatWillSucceed()
    {
        #region MessageThatWillSucceed

        var message = @"{ Property: 'Hello from NativeEndpoint' }";

        #endregion

        return SendMessage(message);
    }

    static Task SendMessageThatWillFail()
    {
        #region MessageThatWillFail

        var message = @"{ invalid json }";

        #endregion

        return SendMessage(message);
    }

    #region sendMessage

    static async Task SendMessage(string messageBody)
    {
        var headers = new Dictionary<string, string>
        {
            {Headers.EnclosedMessageTypes, "SendMessage"}
        };
        var message = new OutgoingMessage(
            id: Guid.NewGuid(),
            correlationId: null,
            replyToAddress: "NativeEndpoint",
            expires: null,
            headers: Headers.Serialize(headers),
            bodyBytes: Encoding.UTF8.GetBytes(messageBody));
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            var queueManager = new QueueManager("NsbEndpoint", connection);
            await queueManager.Send(message)
                .ConfigureAwait(false);
        }
    }

    #endregion

    static async Task CreateQueue()
    {
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            var queueManager = new QueueManager("NativeEndpoint", connection);
            await queueManager.Create()
                .ConfigureAwait(false);
        }
    }
}