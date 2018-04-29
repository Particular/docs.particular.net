using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class Usage
{
    string connectionString = null;

    async Task QueueManagement()
    {
        #region CreateQueue

        await QueueCreator.Create(
            connection: connectionString,
            table: "endpointTable").ConfigureAwait(false);

        #endregion

        #region DeleteQueue

        await SqlHelpers.Drop(
            connection: connectionString,
            table: "endpointTable").ConfigureAwait(false);

        #endregion

        #region CreateDelayedQueue

        await QueueCreator.CreateDelayed(
            connection: connectionString,
            table: "endpointTable.Delayed").ConfigureAwait(false);

        #endregion

        #region DeleteDelayedQueue

        await SqlHelpers.Drop(
            connection: connectionString,
            table: "endpointTable.Delayed").ConfigureAwait(false);

        #endregion
    }

    async Task Send()
    {
        string headers = null;
        byte[] body = null;

        #region Send

        var sender = new Sender("endpointTable");
        var message = new OutgoingMessage(
            id: Guid.NewGuid(),
            headers: headers,
            bodyBytes: body);
        await sender.Send(connectionString, message)
            .ConfigureAwait(false);

        #endregion

    }

    async Task SendBatch()
    {
        string headers1 = null;
        byte[] body1 = null;
        string headers2 = null;
        byte[] body2 = null;

        #region SendBatch

        var sender = new Sender("endpointTable");
        var messages = new List<OutgoingMessage>
        {
            new OutgoingMessage(
                id: Guid.NewGuid(),
                headers: headers1,
                bodyBytes: body1),
            new OutgoingMessage(
                id: Guid.NewGuid(),
                headers: headers2,
                bodyBytes: body2),
        };
        await sender.Send(connectionString, messages)
            .ConfigureAwait(false);

        #endregion

    }

    async Task SendDelayed()
    {
        string headers = null;
        byte[] body = null;

        #region SendDelayed

        var sender = new DelayedSender("endpointTable.Delayed");
        var message = new OutgoingDelayedMessage(
            due: DateTime.UtcNow.AddDays(1),
            headers: headers,
            bodyBytes: body);
        await sender.Send(connectionString, message)
            .ConfigureAwait(false);

        #endregion

    }

    async Task SendDelayedBatch()
    {
        string headers1 = null;
        byte[] body1 = null;
        string headers2 = null;
        byte[] body2 = null;

        #region SendDelayedBatch

        var sender = new DelayedSender("endpointTable.Delayed");
        var messages = new List<OutgoingDelayedMessage>
        {
            new OutgoingDelayedMessage(
                due: DateTime.UtcNow.AddDays(1),
                headers: headers1,
                bodyBytes: body1),
            new OutgoingDelayedMessage(
                due: DateTime.UtcNow.AddDays(1),
                headers: headers2,
                bodyBytes: body2),
        };
        await sender.Send(connectionString, messages)
            .ConfigureAwait(false);

        #endregion

    }

    async Task Read()
    {
        #region Read

        var reader = new Reader("endpointTable");
        var incomingBytesMessage = await reader.ReadBytes(
                connection: connectionString,
                rowVersion: 10)
            .ConfigureAwait(false);

        Console.WriteLine(incomingBytesMessage.Headers);
        Console.WriteLine(incomingBytesMessage.Body);

        #endregion
    }

    async Task ReadBatch()
    {
        #region ReadBatch

        var reader = new Reader("endpointTable");
        var result = await reader.ReadBytes(
                connection: connectionString,
                size: 5,
                startRowVersion: 10,
                action: message =>
                {
                    Console.WriteLine(message.Headers);
                    Console.WriteLine(message.Body);
                })
            .ConfigureAwait(false);

        Console.WriteLine(result.Count);
        Console.WriteLine(result.LastRowVersion);

        #endregion
    }

    async Task RowTracking()
    {

        long newRowVersion = 0;

        #region RowVersionTracker

        var versionTracker = new RowVersionTracker(connectionString);

        // create table
        await versionTracker.CreateTable(connectionString)
            .ConfigureAwait(false);

        // save row version
        await versionTracker.Save(connectionString, newRowVersion)
            .ConfigureAwait(false);

        // get row version
        var startingRow = await versionTracker.Get(connectionString)
            .ConfigureAwait(false);

        #endregion
    }

    async Task ReadLoop()
    {
        #region ProcessingLoop

        var rowVersionTracker = new RowVersionTracker();

        var startingRow = await rowVersionTracker.Get(connectionString).ConfigureAwait(false);

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
            return rowVersionTracker.Save(connectionString, rowVersion, token);
        }

        var processingLoop = new MessageProcessingLoop(
            table: "error",
            delay: TimeSpan.FromSeconds(1),
            connection: connectionString,
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

    async Task Consume()
    {
        #region Consume

        var consumer = new Consumer("endpointTable");
        var incomingBytesMessage = await consumer.ConsumeBytes(
                connection: connectionString)
            .ConfigureAwait(false);

        Console.WriteLine(incomingBytesMessage.Headers);
        Console.WriteLine(incomingBytesMessage.Body);

        #endregion
    }

    async Task ConsumeBatch()
    {
        #region ConsumeBatch

        var consumer = new Consumer("endpointTable");
        var result = await consumer.ConsumeBytes(
                connection: connectionString,
                size: 5,
                action: message =>
                {
                    Console.WriteLine(message.Headers);
                    Console.WriteLine(message.Body);
                })
            .ConfigureAwait(false);

        Console.WriteLine(result.Count);
        Console.WriteLine(result.LastRowVersion);

        #endregion
    }

    async Task ConsumeLoop()
    {
        #region ConsumeLoop

        Task Callback(IncomingBytesMessage incomingMessage, CancellationToken cancellation)
        {
            var bodyText = Encoding.UTF8.GetString(incomingMessage.Body);
            Console.WriteLine($"Reply received:\r\n{bodyText}");
            return Task.CompletedTask;
        }

        void ErrorCallback(Exception exception)
        {
            Environment.FailFast("Message consuming loop failed", exception);
        }

        // start consuming
        var consumingLoop = new MessageConsumingLoop(
            table: "endpointTable",
            delay: TimeSpan.FromSeconds(1),
            connection: connectionString,
            callback: Callback,
            errorCallback: ErrorCallback);
        consumingLoop.Start();

        // stop consuming
        await consumingLoop.Stop().ConfigureAwait(false);

        #endregion
    }


    void Serialize()
    {
        #region Serialize

        var headers = new Dictionary<string, string>
        {
            {Headers.EnclosedMessageTypes, "SendMessage"}
        };
        var serialized = Headers.Serialize(headers);

        #endregion
    }

    void Deserialize()
    {
        string headersString = null;

        #region Deserialize

        var headers = Headers.DeSerialize(headersString);

        #endregion
    }

}