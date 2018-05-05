using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class Usage
{
    SqlConnection sqlConnection = null;

    async Task CreateQueue()
    {
        #region CreateQueue

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        await queueManager.Create().ConfigureAwait(false);

        #endregion
    }

    async Task DeleteQueue()
    {
        #region DeleteQueue

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        await queueManager.Drop().ConfigureAwait(false);

        #endregion
    }

    async Task CreateDelayedQueue()
    {
        #region CreateDelayedQueue

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        await queueManager.Create().ConfigureAwait(false);

        #endregion
    }

    async Task DeleteDelayedQueue()
    {
        #region DeleteDelayedQueue

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        await queueManager.Drop().ConfigureAwait(false);

        #endregion
    }

    async Task Send()
    {
        string headers = null;
        byte[] body = null;

        #region Send

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        var message = new OutgoingMessage(
            id: Guid.NewGuid(),
            headers: headers,
            bodyBytes: body);
        await queueManager.Send(message)
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

        var queueManager = new QueueManager("endpointTable", sqlConnection);
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
        await queueManager.Send(messages)
            .ConfigureAwait(false);

        #endregion
    }

    async Task SendDelayed()
    {
        string headers = null;
        byte[] body = null;

        #region SendDelayed

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        var message = new OutgoingDelayedMessage(
            due: DateTime.UtcNow.AddDays(1),
            headers: headers,
            bodyBytes: body);
        await queueManager.Send(message)
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

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
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
        await queueManager.Send(messages)
            .ConfigureAwait(false);

        #endregion
    }

    async Task Read()
    {
        #region Read

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        var incomingBytesMessage = await queueManager.ReadBytes(rowVersion: 10)
            .ConfigureAwait(false);

        Console.WriteLine(incomingBytesMessage.Headers);
        Console.WriteLine(incomingBytesMessage.Body);

        #endregion
    }

    async Task ReadBatch()
    {
        #region ReadBatch

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        var result = await queueManager.ReadBytes(
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

    async Task Consume()
    {
        #region Consume

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        var incomingBytesMessage = await queueManager.ConsumeBytes()
            .ConfigureAwait(false);

        Console.WriteLine(incomingBytesMessage.Headers);
        Console.WriteLine(incomingBytesMessage.Body);

        #endregion
    }

    async Task ConsumeBatch()
    {
        #region ConsumeBatch

        var queueManager = new QueueManager("endpointTable", sqlConnection);
        var result = await queueManager.ConsumeBytes(
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
}